using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Common.Services
{
    public class CommentService
    {
       
        private const string ITEM_RATING_CT_NAME = "Item Rating ContentType";
        private const string ITEM_COMMENT_CT_NAME = "Item Comment ContentType";


        public static CommentItem GetComment(int id)
        {
            return GetComment(id, SPContext.Current.Web);
        }

        public static CommentItem GetComment(int id, SPWeb web)
        {
            var listComment = CCIUtility.GetListFromURL(Constants.COMMENT_LIST_URL, web);
            var query = new CAMLListQuery<CommentItem>(listComment);
            return query.GetItemById(id);
        }

        public static List<Model.Entities.CommentItem> GetComments(string url, SPWeb web)
        {
            var listComment = CCIUtility.GetListFromURL(Constants.COMMENT_LIST_URL, web);
            return GetComments(url, listComment);
        }

        public static List<Model.Entities.CommentItem> GetComments(string url, SPList listComment)
        {
            url = url.TrimQueryString();
            
            var query = new CAMLListQuery<CommentItem>(listComment);
            string caml = Camlex.Query()
                              .Where(x => (string)x["CommentUrl"] == url 
                                  && x[SPBuiltInFieldId.ContentType] == (DataTypes.Computed)ITEM_COMMENT_CT_NAME 
                                  && x[IOfficeColumnId.ReplyTo] == null)
                              .OrderBy(x => x[SPBuiltInFieldId.ID] as Camlex.Desc)
                  
                              .ToString();
                       

            return query.ExecuteListQuery(caml);
        }

        public static List<Model.Entities.CommentItem> GetComments(string url, int parentId, SPWeb web)
        {
            return GetComments(url, parentId, Constants.COMMENT_LIST_URL, web);
        }

        public static List<Model.Entities.CommentItem> GetComments(string url, int parentId, string commentListUrl, SPWeb web)
        {
            url = url.TrimQueryString();

            var listComment = CCIUtility.GetListFromURL(commentListUrl, web);

            if (parentId <= 0)
                return GetComments(url, listComment);

            var query = new CAMLListQuery<CommentItem>(listComment);
            string caml = Camlex.Query()
                              .Where(x => (string)x["CommentUrl"] == url &&
                                          //x[SPBuiltInFieldId.ContentType] == (DataTypes.Computed)ITEM_COMMENT_CT_NAME &&
                                          x[IOfficeColumnId.ReplyTo] == (DataTypes.LookupId) parentId.ToString())
                              .OrderBy(x => x[SPBuiltInFieldId.ID] as Camlex.Desc)

                              .ToString();


            return query.ExecuteListQuery(caml);
        }

        public static CommentItem Rate(string source, decimal rate, string loginName)
        {
            return Rate(source, rate, loginName, Constants.COMMENT_LIST_URL);
        }

        public static CommentItem Rate(string source, decimal rate, string loginName, string commentListUrl)
        {
            source = source.TrimQueryString();
             decimal newrate  = 0.0M;
             var list = CCIUtility.GetListFromURL(Constants.COMMENT_LIST_URL, SPContext.Current.Web);
            CommentItem result = null;
             try
             {
                 list.ParentWeb.AllowUnsafeUpdates = true;
                 var query = new CAMLListQuery<CommentItem>(list);
                 string caml =
                 Camlex.Query()
                       .Where(x => (string)x["CommentUrl"] == source &&
                                   x[SPBuiltInFieldId.ContentType] == (DataTypes.Computed)ITEM_RATING_CT_NAME).ToString();

                 var item = query.GetItem(caml);
                 if (item == null)
                 {
                     item = list.AddItem();
                 }
                 else
                 {
                     item = list.GetItemById(item.ID);
                 }
                 string users = item[IOfficeColumnId.RatingUsers] == null ? string.Empty : item[IOfficeColumnId.RatingUsers].ToString();
                 List<RateInfo> ratedList = new List<RateInfo>();
                 var currentInfo = new RateInfo()
                 {
                     RateValue = rate,
                     User = loginName
                 };

                 if (string.IsNullOrEmpty(users))
                 {
                     ratedList.Add(currentInfo);
                 }
                 else
                 {
                     ratedList = SerializationHelper.DeserializeFromXml<List<RateInfo>>(users);
                     var existed = ratedList.Where(p => p.User == loginName).FirstOrDefault();
                     if (existed != null)
                     {
                         existed.RateValue = rate;
                     }
                     else
                     {
                         ratedList.Add(currentInfo);
                     }
                 }

                 newrate = ratedList.Average(p => p.RateValue);
                 item[IOfficeColumnId.RatingAvg] = newrate;
                 item[IOfficeColumnId.CommentUrl] = source;
                 item[IOfficeColumnId.RatingUsers] = SerializationHelper.SerializeToXml<List<RateInfo>>(ratedList);
                 item[SPBuiltInFieldId.ContentTypeId] = new SPContentTypeId(IOfficeContentType.ITEM_RATING_CONTENTTYPE);
                 item.SystemUpdate();
                 result= new CommentItem(item);
                 result.RatedList = ratedList;
             }
             catch (Exception ex)
             {
                 CCIUtility.LogError(ex);

             }
             finally
             {
                 list.ParentWeb.AllowUnsafeUpdates = false;
             }
             return result;
        }

        public static CommentItem GetRateItemByUrl(string url)
        {
            return GetRateItemByUrl(url, Constants.COMMENT_LIST_URL);
        }

        public static CommentItem GetRateItemByUrl(string url, string commentListUrl)
        {
            CommentItem result = null;
            var list = CCIUtility.GetListFromURL(commentListUrl, SPContext.Current.Web);
            if (list == null) return null;

            try
            {
                list.ParentWeb.AllowUnsafeUpdates = true;
                var query = new CAMLListQuery<CommentItem>(list);
                string caml =
                Camlex.Query()
                      .Where(x => ((string)x["CommentUrl"]).Contains(url) &&
                                  x[SPBuiltInFieldId.ContentType] == (DataTypes.Computed)ITEM_RATING_CT_NAME).ToString();

                 result = query.ExecuteSingleQuery(caml);
                
              
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex);

            }
            finally
            {
                list.ParentWeb.AllowUnsafeUpdates = false;
            }
            return result;
        }
    }
}
