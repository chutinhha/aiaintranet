using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using AIA.Intranet.Model.Entities;
using System.Collections.Generic;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Resources;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.WebParts.ViewNewsCategoryWebPart
{
    public partial class ViewNewsCategoryWebPartUserControl : UserControl
    {
        #region [Properties]
        public ViewNewsCategoryWebPart WebPart { get; set; }

        public SPList ListNewsByCategory
        {
            get
            {
                SPList listNews = null;

                try
                {
                    if (WebPart != null)
                    {
                        if (!string.IsNullOrEmpty(WebPart.ListID))
                        {
                            if (!string.IsNullOrEmpty(WebPart.WebID))
                            {
                                using (SPWeb spWeb = SPContext.Current.Site.OpenWeb(new Guid(WebPart.WebID)))
                                {
                                    listNews = spWeb.Lists[new Guid(WebPart.ListID)];
                                }
                            }
                            else
                            {
                                listNews = SPContext.Current.Web.Lists[new Guid(WebPart.ListID)];
                            }
                        }
                        else if (!string.IsNullOrEmpty(WebPart.ListUrl))
                        {
                            listNews = Utility.GetListFromURL(WebPart.ListUrl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.LogError(ex);
                }

                return listNews;
            }
        }

        public int ItemCount
        {
            get
            {
                int number = 4;

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
                string d = CommonResources.ENDateTimeFormat;

                if (WebPart != null && !string.IsNullOrEmpty(WebPart.DateTimeFormat))
                {
                    d = WebPart.DateTimeFormat;
                }

                return d;
            }
        }

        public string MainPicWidth
        {
            get
            {
                string w = "300px";

                if (WebPart != null)
                {
                    w = WebPart.MainPicWidth.IndexOf("px") != -1 ? WebPart.MainPicWidth : WebPart.MainPicWidth.Trim() + "px"; ;
                }

                return w;
            }
        }
        #endregion [Properties]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DisplayNewsByCategory();
            }
        }

        private void DisplayNewsByCategory()
        {
            if (ListNewsByCategory != null)
            {
                List<NewsItem> latestNews = NewsService.GetLatestNewsByCategory(ListNewsByCategory, ItemCount);

                if (latestNews != null && latestNews.Count > 0)
                {
                    //bind main news
                    var firstNews = latestNews[0];
                    StringBuilder htmlFirstNews = new StringBuilder();

                    htmlFirstNews.AppendFormat("<div class='wp-catnews-main-title-content'><a href='{0}' class='wp-catnews-title'>{1}</a>", firstNews.ViewUrl, firstNews.Title);
                    if (ShowDateTime)
                    {
                        htmlFirstNews.AppendFormat(" <span class='wp-catnews-date'>({0})</span>", firstNews.CreatedDate.ToString(DateTimeFormat));
                    }
                    htmlFirstNews.Append("</div><div>");

                    if (!string.IsNullOrEmpty(firstNews.Thumbnail))
                    {
                        htmlFirstNews.AppendFormat("<a href='{0}'><img src='{1}' align='left' style='width:{2}' alt=''/></a>", firstNews.ViewUrl, firstNews.Thumbnail, MainPicWidth);
                    }

                    htmlFirstNews.AppendFormat("<span class='wp-catnews-desc'>{0}</span></div>", firstNews.ShortDescription);

                    ltMainNews.Text = htmlFirstNews.ToString();

                    //bind other news
                    if (latestNews.Count > 1)
                    {
                        StringBuilder htmlOtherNews = new StringBuilder();
                        for (int i = 1; i < latestNews.Count; i++)
                        {
                            var otherNews = latestNews[i];

                            htmlOtherNews.AppendFormat("<li><a class='wp-catnews-new-title' href='{0}'>{1}</a>", otherNews.ViewUrl, otherNews.Title);

                            if (ShowDateTime)
                            {
                                htmlOtherNews.AppendFormat(" <span class='wp-catnews-new-date'>({0})</span>", otherNews.CreatedDate.ToString(DateTimeFormat));
                            }

                            htmlOtherNews.Append("</li>");
                        }

                        ltOtherNews.Text = htmlOtherNews.ToString();
                    }
                }
            }
        }
    }
}
