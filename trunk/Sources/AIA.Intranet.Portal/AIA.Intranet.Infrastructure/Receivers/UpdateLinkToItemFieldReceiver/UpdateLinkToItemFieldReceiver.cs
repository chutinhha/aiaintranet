using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Receivers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class UpdateLinkToItemFieldReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            if (properties.ListItem == null)
                return;
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                foreach (SPField field in properties.ListItem.Fields)
                {
                    var item = properties.ListItem;

                    if (field.TypeAsString == "LinkToItem")
                    {
                        // Edit form full url
                        //string.Format("{0}{1}?ID={2}", item.Web.Url, item.ParentList.Forms[PAGETYPE.PAGE_EDITFORM].ServerRelativeUrl, item.ID);

                        // Edit form relative url
                        //string.Format("{0}?ID={1}", item.ParentList.Forms[PAGETYPE.PAGE_EDITFORM].ServerRelativeUrl, item.ID);

                        // Display form full url
                        //string url = string.Format("{0}{1}?ID={2}", item.Web.Site.RootWeb.Url, item.ParentList.DefaultDisplayFormUrl, item.ID);
                        //should use this instead: PageType=4
                        string url = string.Format("{0}/_layouts/listform.aspx?PageType=4&ListId={1}&ID={2}", item.Web.Url, item.ParentList.ID.ToString(), item.ID);

                        // Display form relative url
                        //string.Format("{0}?ID={1}", item.ParentList.Forms[PAGETYPE.PAGE_DISPLAYFORM].ServerRelativeUrl, item.ID);
                        properties.ListItem[field.Id] = url;
                        properties.ListItem.SystemUpdate();
                    }
                }
                //base.ItemAdded(properties);  
            }
        }


    }
}
