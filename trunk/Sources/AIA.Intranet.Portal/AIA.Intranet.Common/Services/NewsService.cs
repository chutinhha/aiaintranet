using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex;
using System.Linq.Expressions;

namespace AIA.Intranet.Common.Services
{
    public class NewsService
    {
        //public static List<Model.Entities.NewsItem> GetLastestNews(SPWeb web, int itemCount, bool hotNewsOnly)
        //{
        //    CAMLSiteQuery<NewsItem> query = new CAMLSiteQuery<NewsItem>(ListTemplateIds.NEWS_TEMPLATE_ID, web);

        //    string caml = string.Empty;

        //    if (hotNewsOnly)
        //    {
        //        var orderByList = new List<Expression<Func<SPListItem, object>>>();
        //        orderByList.Add(x => x["OrderNumber"] as Camlex.Asc);
        //        orderByList.Add(x => x[SPBuiltInFieldId.Created] as Camlex.Desc);
        //         caml = Camlex.Query()
        //                      .Where(x => (bool)x["IsHotNews"] == true &&
        //                                  x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
        //                      .OrderBy(x => x["OrderNumber"] as Camlex.Desc)
        //                      .ToString();
        //    }

        //    return query.ExecuteListQuery(caml, itemCount);
        //}

        public static List<Model.Entities.NewsItem> GetLastestNews(SPList list, int itemCount, bool hotNewsOnly)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            if (hotNewsOnly)
            {
                var orderByList = new List<Expression<Func<SPListItem, object>>>();
                orderByList.Add(x => x["OrderNumber"] as Camlex.Asc);
                orderByList.Add(x => x[SPBuiltInFieldId.Created] as Camlex.Desc);
                caml = Camlex.Query()
                             .Where(x => (bool)x["IsHotNews"] == true &&
                                         x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                             .OrderBy(x => x["OrderNumber"] as Camlex.Desc)
                             .ToString();
            }

            return query.ExecuteListQuery(caml, itemCount);
        }

        public static List<Model.Entities.NewsItem> GetLastestNews(int itemCount)
        {
            var newsList = CCIUtility.GetListFromURL(Constants.NEWS_LIST_URL, SPContext.Current.Web);
            if (newsList != null)
                return GetLastestNews(newsList, itemCount, false);
            return null;
            
        }

        public static List<Model.Entities.NewsItem> GetLastestNews(int itemCount, bool hotNewsOnly)
        {
            var newsList = CCIUtility.GetListFromURL(Constants.NEWS_LIST_URL, SPContext.Current.Web);
            if (newsList != null)
                return GetLastestNews(newsList, itemCount, hotNewsOnly);
            return null;
        }

        public static List<Model.Entities.NewsItem> GetLatestNewsByCategory(SPList list, int itemCount)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount);
        }

        public static List<Model.Entities.NewsItem> GetNewsByCategory(SPList list, int itemCount, string pagingInfo, out SPListItemCollection collection)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount, pagingInfo, out collection);
        }

        public static Model.Entities.NewsItem GetNewsById(SPList list, int id)
        { 
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            return query.GetItemById(id);
        }

        public static Model.Entities.NewsItem GetLatestNewsInCategory(SPList list)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, 1).FirstOrDefault();
        }

        public static Model.Entities.NewsItem CreateInstance(SPList list, SPListItem item)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            return query.CreateInstance(item);
        }

        public static Model.Entities.NewsItem CreateInstance(SPListItem item)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(item.ParentList);

            return query.CreateInstance(item);
        }

        public static List<Model.Entities.NewsItem> GetOtherNews(SPList list, int exceptId, int itemCount)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => (int)x[SPBuiltInFieldId.ID] != exceptId && x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount);
        }
    }
}
