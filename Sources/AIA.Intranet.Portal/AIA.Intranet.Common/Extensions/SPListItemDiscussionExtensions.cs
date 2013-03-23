using System;
using System.Text;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Models.Infrastructure;
namespace AIA.Intranet.Common.Extensions
{
    [CLSCompliant(false)]
    public static class SPListItemDiscussionExtensions
    {
        public static bool DiscussionSetupValid(this SPListItem item)
        {
            bool isCorrect = false;

            if (item != null)
            {
                // Check to make sure the fields and List Exist.
                isCorrect = 
                    item.ParentList.DiscussionsEnabledGet() && 
                    item.ParentList.DiscussionBoardList() != null;
            }

            return isCorrect;
        }
        public static SPListItem DiscussionThreadItem(this SPListItem item, bool createIfDNE)
        {
            SPList discussionList = item.ParentList.DiscussionBoardList();
            SPListItem refreshedItem = item.ParentList.GetItemById(item.ID); // This is a little strange, but it makes sure we have the LATEST item since an elevated block may have changed it.
            SPListItem discussionItem = null;

            if (discussionList != null) // If we have a discussion list proceed.
            {
                int discussionItemId = refreshedItem.DiscussionThreadId();

                if (discussionItemId == -1 && createIfDNE)
                {
                    refreshedItem.DiscussionThreadCreate();
                    
                    // Now that it was created, call this method back but don't try to create (since it already should be).
                    discussionItem = refreshedItem.DiscussionThreadItem(false);
                }
                else
                {
                    try
                    {
                        discussionItem = discussionList.GetItemById(discussionItemId);
                    }
                    catch (Exception ex)
                    {
                        //CCIUtility.LogError("List item ID is not exist in target list", "AIA.Intranet.ItemDiscussion");
                        if (createIfDNE)
                        {
                            refreshedItem.DiscussionThreadCreate();

                            // Now that it was created, call this method back but don't try to create (since it already should be).
                            discussionItem = refreshedItem.DiscussionThreadItem(false);
                        }
                    }
                }
            }

            return discussionItem;
        }

        public static int DiscussionThreadId(this SPListItem item)
        {
            return item.Properties[ItemDiscussionProperties.ItemDiscussionId] == null ? -1 : Convert.ToInt32(item.Properties[ItemDiscussionProperties.ItemDiscussionId]);
        }

        public static int DiscussionThreadCreate(this SPListItem item)
        {
            int createdThreadId = -1;

            // Get the discussion board that is associated with this list.
            SPList discussionList = item.ParentList.DiscussionBoardList();

            if (discussionList != null)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(item.Web.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(item.Web.ID))
                        {
                            try
                            {
                                // Required in this case since the http request is a get.
                                web.AllowUnsafeUpdates = true;

                                // Get the elevated item.
                                SPList itemListElevated = web.Lists[item.ParentList.ID];
                                SPListItem itemElevated = itemListElevated.GetItemById(item.ID);

                                // Create a new thread based on the name of the document.
                                SPList discussionListElevated = web.Lists[discussionList.ID];
                                SPListItem newThread = discussionListElevated.Items.Add();
                                newThread["Title"] = itemElevated.DisplayName;
                                newThread["Body"] = itemElevated.DicussionThreadBody();
                                newThread.SystemUpdate();

                                // Get the ID of the newly created thread;
                                createdThreadId = newThread.ID;

                                // Now update the document with the ID of the Discussion Thread.
                                itemElevated.Properties[ItemDiscussionProperties.ItemDiscussionId] = newThread.ID;
                                itemElevated.SystemUpdate(); 
                                //TODO: This makes the event fire again, which in this case does not hurt anything, but should be changed.
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });
            }
            else
            {

                //TODO: Here is a perfect place to add logging.
            }

            return createdThreadId;
        }

        public static string DicussionThreadBody(this SPListItem item)
        {
            // Build the body that will be used in the thread.
            StringBuilder body = new StringBuilder();
            body.Append(@"<table>"); 
            body.AppendFormat(@"<tr><td><strong>Name:</strong></td><td>&nbsp;</td><td>{0}</td></tr>", item.DisplayName);
            body.AppendFormat(@"<tr><td><strong>View Properties:</strong></td><td>&nbsp;</td><td><a href='{0}'>{1}</a></td></tr>", item.DisplayFormUrl(), item.Url);
            body.Append(@"</table>");
            body.Append(@"<br/><div>Reply to this post to start a discussion around this item.</div>");

            return body.ToString();
        }

    }
}
