using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using AIA.Intranet.Common.Services;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Entities;

namespace AIA.Intranet.Infrastructure.WebParts.HomeNews
{
    public partial class HomeNewsUserControl : UserControl
    {
        public string WebPartTitle { get; set; }
        public int NumberOfItem { get; set; }
        public List<CommingUpLink> CommingUpLink { get; set; }

        protected override void OnInit(EventArgs e)
        {
            repeaterCommingUp.ItemDataBound += new RepeaterItemEventHandler(repeaterCommingUp_ItemDataBound);
            repeaterHotNews.ItemDataBound += new RepeaterItemEventHandler(repeaterHotNews_ItemDataBound);
            base.OnInit(e);
        }

        void repeaterHotNews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            NewsItem rowView = (NewsItem)e.Item.DataItem;
            if (rowView != null)
            {
                HyperLink hyperLinkTitle = e.Item.FindControl("hyperLinkTitle") as HyperLink;
                Label lableDate = e.Item.FindControl("lableDate") as Label;
                hyperLinkTitle.Text = rowView.Title;
                hyperLinkTitle.NavigateUrl = rowView.ViewUrl;
                lableDate.Text = rowView.ModifiedDate.ToString("dd/MM/yyyy HH:mm");
            }
        }

        void repeaterCommingUp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CommingUpLink rowView = (CommingUpLink)e.Item.DataItem;
            if (rowView != null)
            {
                HyperLink hyperLinkCommingUp = e.Item.FindControl("hyperLinkCommingUp") as HyperLink;
                hyperLinkCommingUp.Text = rowView.Title;
                hyperLinkCommingUp.NavigateUrl = rowView.Link;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommingUpLink != null && CommingUpLink.Count > 0)
                {
                    repeaterCommingUp.DataSource = CommingUpLink;
                    repeaterCommingUp.DataBind();
                }

                List<Model.Entities.NewsItem> newsItems = NewsService.GetLastestNews(SPContext.Current.Web, NumberOfItem, true);
                if (newsItems != null && newsItems.Count > 0)
                {
                    repeaterHotNews.DataSource = newsItems;
                    repeaterHotNews.DataBind();
                }
            }
        }
    }
}
