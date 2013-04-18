using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Services;
using System.Collections.Generic;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Infrastructure.WebParts.NewsListView
{
    public partial class NewsListViewUserControl : UserControl
    {
        #region [Properties]
        public NewsListView WebPart { get; set; }

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
                string w = "150px";

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
            if (!IsPostBack)
            {
                LoadData(1, string.Empty);
            }
        }

        private void LoadData(int currentPage, string pagingInfo)
        {
            ViewState["CurrentPage"] = currentPage;
            FillData(pagingInfo, "Created", false);
        }

        protected void FillData(string pagingInfo, string sortColumn, bool sortAscending)
        {
            int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
            int rowCount = ItemCount;
            string columnValue;
            string nextPageString = "Paged=TRUE&p_ID={0}&p_" + sortColumn + "={1}";
            string previousPageString = "Paged=TRUE&PagedPrev=TRUE&p_ID={0}&p_" + sortColumn + "={1}";

            SPListItemCollection collection;

            SPList list = SPContext.Current.List;

            List<NewsItem> catNews = NewsService.GetNewsByCategory(list, rowCount, pagingInfo, out collection);

            if (catNews != null && catNews.Count > 0)
            {
                rptNews.DataSource = catNews;
                rptNews.DataBind();

                //identify if this is a call from next or first
                if (collection != null && collection.ListItemCollectionPosition != null)
                {
                    if (collection.Fields[sortColumn].Type == SPFieldType.DateTime)
                    {
                        columnValue = SPEncode.UrlEncode(Convert.ToDateTime(collection[collection.Count - 1][sortColumn]).ToUniversalTime().ToString("yyyyMMdd HH:mm:ss"));
                    }
                    else
                    {
                        columnValue = SPEncode.UrlEncode(Convert.ToString(collection[collection.Count - 1][sortColumn]));
                    }

                    nextPageString = string.Format(nextPageString, collection[collection.Count - 1].ID, columnValue);
                }
                else
                {
                    nextPageString = string.Empty;
                }

                if (currentPage > 1)
                {

                    if (collection.Fields[sortColumn].Type == SPFieldType.DateTime)
                    {
                        columnValue = SPEncode.UrlEncode(Convert.ToDateTime(collection[0][sortColumn]).ToUniversalTime().ToString("yyyyMMdd HH:mm:ss"));
                    }
                    else
                    {
                        columnValue = SPEncode.UrlEncode(Convert.ToString(collection[0][sortColumn]));
                    }

                    previousPageString = string.Format(previousPageString, collection[0].ID, columnValue);
                }
                else
                {
                    previousPageString = string.Empty;
                }

                if (string.IsNullOrEmpty(nextPageString))
                {
                    ibtnNext.Visible = false;
                }
                else
                {
                    ibtnNext.Visible = true;
                }

                if (string.IsNullOrEmpty(previousPageString))
                {
                    ibtnPrev.Visible = false;
                }
                else
                {
                    ibtnPrev.Visible = true;
                }

                ViewState["Previous"] = previousPageString;
                ViewState["Next"] = nextPageString;
                lblPage.Text = ((currentPage - 1) * rowCount) + 1 + " - " + (currentPage * rowCount - (rowCount - catNews.Count));
            }
        }

        protected void ibtnPrev_Click(object sender, ImageClickEventArgs e)
        {
            LoadData(Convert.ToInt32(ViewState["CurrentPage"]) - 1, ViewState["Previous"] as string);
        }

        protected void ibtnNext_Click(object sender, ImageClickEventArgs e)
        {
            LoadData(Convert.ToInt32(ViewState["CurrentPage"]) + 1, ViewState["Next"] as string);
        }
    }
}
