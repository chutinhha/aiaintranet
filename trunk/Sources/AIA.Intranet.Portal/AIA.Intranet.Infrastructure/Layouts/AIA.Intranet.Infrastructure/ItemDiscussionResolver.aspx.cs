using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Common.Extensions;
using System.Web;

namespace AIA.Intranet.Infrastructure.Layouts
{
    [CLSCompliant(false)]
    public partial class ItemDiscussionResolver : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPSite contextSite = SPContext.Current.Site;
            SPWeb contextWeb = SPContext.Current.Web;

            string listId = Request.QueryString["List"];
            string itemId = Request.QueryString["Item"];

            // The thread item that is linked to the current item.
            SPListItem itemThread = null;

            //SPSecurity.RunWithElevatedPrivileges(delegate
            //{
            using (SPSite site = new SPSite(contextSite.ID))
            {
                using (SPWeb web = site.OpenWeb(contextWeb.ID))
                {
                    SPList itemList = web.Lists[new Guid(listId)];
                    SPListItem item = itemList.Items.GetItemById(Convert.ToInt32(itemId));

                    // If the item has discussions configured correctly then we can proceed.
                    if (item.DiscussionSetupValid())
                    {
                        // Get the thread associated with this item. (Create if it does not exist).
                        itemThread = item.DiscussionThreadItem(true);
                    }
                }
            }
            //});

            string redirectUrl = "_layouts/AIA.Intranet.Infrastructure/ItemDiscussionsNotConfigured.aspx";
            if (itemThread != null && !String.IsNullOrEmpty(itemThread.Url))
                redirectUrl = itemThread.Url;

            this.Redirect(redirectUrl);
        }

        private void Redirect(string discussionItemUrl)
        {
            //TODO: Need better handling here for when the discussionThreadUrl is blank.
            string webRelativeItemUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.ServerRelativeUrl, discussionItemUrl);
            SPUtility.Redirect(webRelativeItemUrl, SPRedirectFlags.Default, HttpContext.Current);
        }
    }    
}
