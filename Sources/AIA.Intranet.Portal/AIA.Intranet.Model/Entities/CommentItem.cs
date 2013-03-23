using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    public class RateInfo
    {
        public string User { get; set; }
        public decimal RateValue { get; set; }
    }
    [Serializable]
    public class CommentItem : BaseEntity
    {
        public string CommentText { get; set; }

        public CommentItem()
        {
            RatedList = new List<RateInfo>();
        }

        public CommentItem(DataRow item)
            : base(item)
        {
            try
            {
                RatedList = new List<RateInfo>();
                
            }
            catch (Exception ex)
            {

            }
         
        }

        public CommentItem(SPListItem item)
            : base(item)
        {
            try
            {
                CommentedBy = base.CreatedBy.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                CommentedBy =SPUtility.GetFullNameFromLogin(item.ParentList.ParentWeb.Site, base.CreatedBy);
                RatedList = new List<RateInfo>();
            }
            catch (Exception)
            {
                
                
            }
           
        }

        [Field(Ignore=true)]
        public string CommentedBy { get; set; }

        public decimal RatingAvg { get; set; }

        public string RatingUsers { get; set; }

        [Field (FieldName="CommentUrl")]
        public string Url { get; set; }
        [Field(Ignore = true)]
        public List<RateInfo> RatedList{ get; set; }

        public string CommentUrl { get; set; }
    }
}
