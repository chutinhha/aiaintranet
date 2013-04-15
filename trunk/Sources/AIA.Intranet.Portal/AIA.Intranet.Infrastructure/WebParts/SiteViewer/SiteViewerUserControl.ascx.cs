using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace AIA.Intranet.Infrastructure.WebParts.SiteViewer
{
    public partial class SiteViewerUserControl : UserControl
    {
        public string WebPartTitle { get; set; }
        public string WebPartDescription { get; set; }
        public string GroupTitle { get; set; }
        public List<CommingUpLink> CommingUpLink { get; set; }

        private string liItem = @"<li><a href='{0}'>{1}</a></li>";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            repeaterCommingUp.ItemDataBound +=new RepeaterItemEventHandler(repeaterCommingUp_ItemDataBound);
            literalWebPartTitle.Text = this.WebPartTitle;
            literalWebPartDescription.Text = this.WebPartDescription;
            literalGroups.Text = this.GroupTitle;
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

                var subWebs = GetSubWebs(SPContext.Current.Web.CurrentUser.LoginName);
                ulDepartment.InnerHtml = subWebs;
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

        private string GetSubWebs(string loginName)
        {
            string subWebs = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPWebCollection webs = web.Webs;
                            foreach (SPWeb subweb in webs)
                            {
                                if (web.DoesUserHavePermissions(loginName, SPBasePermissions.Open))
                                {
                                    subWebs += string.Format(liItem, subweb.ServerRelativeUrl, subweb.Title);
                                }
                            }
                        }
                    }
                });
                
            }
            catch (Exception ex)
            {
            }
            return subWebs;
        }
    }
}
