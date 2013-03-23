using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    public class NewsItem : BaseEntity
    {
        public NewsItem(SPListItem item)
            : base(item)
        {
            //get image url from ImageField
            if (item[IOfficeColumnId.NewsThumbnail1] != null && !string.IsNullOrEmpty(item[IOfficeColumnId.NewsThumbnail1].ToString()))
            {
                Thumbnail = item[IOfficeColumnId.NewsThumbnail1].ToString().Split(new string[] { ";#" }, StringSplitOptions.None)[1];
            }

            SPList spList = item.ParentList;
            ViewUrl = spList.DefaultDisplayFormUrl.Substring(0, spList.DefaultDisplayFormUrl.LastIndexOf("/")) + "/" + Constants.NEWS_DISPLAYPAGE + ".aspx?Id=" + item.ID;

        }
        public NewsItem(DataRow item)
            : base(item)
        {
            if (item["Thumbnail1"] != null && !string.IsNullOrEmpty(item["Thumbnail1"].ToString()))
            {
                Thumbnail = item["Thumbnail1"].ToString().Split(new string[] { ";#" }, StringSplitOptions.None)[1];
            }

            SPSite spSite = SPContext.Current.Site;
            using(SPWeb spWeb = spSite.OpenWeb(new Guid(item["WebId"].ToString())))
            {
                SPList spList = spWeb.Lists[new Guid(item["ListId"].ToString())];

                ViewUrl = spList.DefaultDisplayFormUrl.Substring(0, spList.DefaultDisplayFormUrl.LastIndexOf("/")) + "/" + Constants.NEWS_DISPLAYPAGE + ".aspx?Id=" + item["ID"];
            }
        }

        public NewsItem()
        {
        }

        public string ShortDescription { get; set; }
        public string Contents { get; set; }
        public int ViewCount { get; set; }
        public string Thumbnail { get; set; }
        public string ViewUrl { get; set; }
        public string Thumbnail1 { get; set; }
        public bool IsHotNews { get; set; }
    }
}
