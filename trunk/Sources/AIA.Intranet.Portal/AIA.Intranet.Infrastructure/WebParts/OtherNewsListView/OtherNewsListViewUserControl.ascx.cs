using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using System.Collections.Generic;
using Microsoft.SharePoint;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Infrastructure.WebParts.OtherNewsListView
{
    public partial class OtherNewsListViewUserControl : UserControl
    {
        #region [Properties]
        public OtherNewsListView WebPart { get; set; }

        public int ItemCount
        {
            get
            {
                int number = 10;

                if (WebPart != null)
                {
                    int.TryParse(WebPart.ItemCount, out number);
                }

                return number;
            }
        }

        public bool ShowDateTime
        {
            get
            {
                bool show = true;

                if (WebPart != null)
                {
                    show = WebPart.ShowDateTime;
                }

                return show;
            }
        }

        public string DateTimeFormat
        {
            get
            {
                string d = CommonResources.VNDateTimeFormat;

                if (WebPart != null && !string.IsNullOrEmpty(WebPart.DateTimeFormat))
                {
                    d = WebPart.DateTimeFormat;
                }

                return d;
            }
        }

        #endregion [Properties]

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowData();
            }
        }

        private void ShowData()
        {
            var currentList = SPContext.Current.List;
            var currentItem = SPContext.Current.ListItem;

            if (currentList != null)
            {
                List<NewsItem> newsList;

                if (currentItem != null)
                {
                    newsList = NewsService.GetOtherNews(currentList, currentItem.ID, ItemCount);
                }
                else
                {
                    NewsItem mainNews;
                    mainNews = NewsService.GetLatestNewsInCategory(currentList);
                    newsList = NewsService.GetOtherNews(currentList, mainNews.ID, ItemCount);
                }

                if (newsList != null && newsList.Count > 0)
                {
                    rptOtherNews.DataSource = newsList;
                    rptOtherNews.DataBind();
                }
            }
        }
    }
}
