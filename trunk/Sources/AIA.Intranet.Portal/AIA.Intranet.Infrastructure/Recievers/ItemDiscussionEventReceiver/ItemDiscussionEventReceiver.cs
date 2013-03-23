using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.Recievers.ItemDiscussionEventReceiver
{
    [CLSCompliant(false)]
    public class ItemDiscussionEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Initializes a new instance of the Microsoft.SharePoint.SPItemEventReceiver class.
        /// </summary>
        public ItemDiscussionEventReceiver()
        {
        }

        /// <summary>
        /// When an item is deleted the related discussion is also deleted.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPItemEventProperties object that represents properties of the event handler.
        /// </param>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            // Only process this event if discussions are enabled.
            if (properties.ListItem.ParentList.DiscussionsEnabledGet())
            {
                // Get the current Item.
                SPListItem currentItem = properties.ListItem;

                //TODO: instead of elevating here, why not create an extension method that returns an elevated item?  Not sure if that would work or not.
                // Elevate privilages here, since the user probably will not have access to the top level discussion item.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(currentItem.Web.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(currentItem.Web.ID))
                        {
                            SPList itemListElevated = web.Lists[currentItem.ParentList.ID];
                            SPListItem itemElevated = itemListElevated.GetItemById(currentItem.ID);

                            if (itemElevated.DiscussionSetupValid())
                            {
                                // Get the thread associated with this item.
                                SPListItem itemThread = itemElevated.DiscussionThreadItem(false);
                                if (itemThread != null)
                                {
                                    itemThread.Delete();
                                }
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Asynchronous after event that occurs after an existing item is changed, for example, when the user changes data in one or more fields.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPItemEventProperties object that represents properties of the event handler.
        /// </param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            // I orginally had code to only update the thread title if doc name changed,
            // but that turned out to not work very well because of the feaky nature of
            // After/Before properties.
            // Only process this event if discussions are enabled.
            if (properties.ListItem.ParentList.DiscussionsEnabledGet())
            {
                // Get the current Item.
                SPListItem currentItem = properties.ListItem;

                // Elevate privilages here, since the user probably will not have access to the top level discussion item.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(currentItem.Web.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(currentItem.Web.ID))
                        {
                            SPList itemListElevated = web.Lists[currentItem.ParentList.ID];
                            SPListItem itemElevated = itemListElevated.GetItemById(currentItem.ID);

                            // Make sure we also have valid discussion board
                            if (itemElevated.DiscussionSetupValid())
                            {
                                // Get the thread associated with this item.
                                SPListItem itemThread = itemElevated.DiscussionThreadItem(false);
                                if (itemThread != null)
                                {
                                    //TODO: this is duplicate of the resolver.  Really should only be one.
                                    itemThread["Title"] = itemElevated.DisplayName;
                                    itemThread["Body"] = itemElevated.DicussionThreadBody();
                                    itemThread.SystemUpdate(false);
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}
