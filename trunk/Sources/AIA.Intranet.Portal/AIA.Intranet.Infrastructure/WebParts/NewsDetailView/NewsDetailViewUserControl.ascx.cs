using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Infrastructure.WebParts.NewsDetailView
{
    public partial class NewsDetailViewUserControl : UserControl
    {
        #region [Properties]
        public NewsDetailView WebPart { get; set; }

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
                string d = CommonResources.ENDateTimeFormat;

                if (WebPart != null && !string.IsNullOrEmpty(WebPart.DateTimeFormat))
                {
                    d = WebPart.DateTimeFormat;
                }

                return d;
            }
        }
        #endregion [Properties]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HideRibbon();
                ShowData();
            }
        }

        private void ShowData()
        {
            var currentList = SPContext.Current.List;
            var currentItem = SPContext.Current.ListItem;

            if (currentList != null)
            {
                NewsItem mainNews;

                if (currentItem != null)
                {
                    mainNews = NewsService.CreateInstance(currentList, currentItem);
                }
                else
                {
                    mainNews = NewsService.GetLatestNewsInCategory(currentList);
                }

                if (mainNews != null)
                {
                    ltNewsTitle.Text = mainNews.Title;
                    ltNewsDate.Text = mainNews.Created.ToString(DateTimeFormat);
                    ltNewsDescription.Text = mainNews.ShortDescription;
                    ltNewsContent.Text = mainNews.Contents;
                    //divContent.InnerHtml = mainNews.Contents;
                }
            }
        }

        private void HideRibbon()
        {
            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);

            if (ribbon != null)
            {
                ribbon.Minimized = true;
            }
        }
    }
}
