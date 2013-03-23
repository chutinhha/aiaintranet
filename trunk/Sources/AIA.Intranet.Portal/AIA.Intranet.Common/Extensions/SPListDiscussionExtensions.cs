using System;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using AIA.Intranet.Infrastructure;
using AIA.Intranet.Models.Infrastructure;

namespace AIA.Intranet.Common.Extensions
{
    [CLSCompliant(false)]
    public static class SPListDiscussionExtensions
    {
        public static bool DiscussionsEnabledGet(this SPList list)
        {
                string isDiscussionEnabledString = list.GetCustomProperty(getWebKeyName(list, ItemDiscussionProperties.IsDiscussionEnabledPopertyName));
                bool isDiscussionEnabled = false;

                bool.TryParse(isDiscussionEnabledString, out isDiscussionEnabled);

                return isDiscussionEnabled;
        }

        public static void DiscussionsEnabledSet(this SPList list, bool value)
        {
            list.SetCustomProperty(getWebKeyName(list, ItemDiscussionProperties.IsDiscussionEnabledPopertyName), value.ToString());
        }

        public static string DiscussionBoardTitleGet(this SPList list)
        {
            return list.GetCustomProperty(getWebKeyName(list, ItemDiscussionProperties.ListDiscussionBoardTitle));
        }

        public static void DiscussionBoardTitleSet(this SPList list, string value)
        {
            list.SetCustomProperty(getWebKeyName(list, ItemDiscussionProperties.ListDiscussionBoardTitle), value);
        }

        public static SPList DiscussionBoardList(this SPList list)
        {
            SPList discussionBoardList = null;
            SPWeb web = list.ParentWeb;            
            string discussionBoardTitle = DiscussionBoardTitleGet(list);

            // If we have a value try to get the list.
            if (!String.IsNullOrEmpty(discussionBoardTitle) && web.ListExists(discussionBoardTitle))
            {
                //TODO: What happens when you rename to list title.  Does that break things?  Should I be using the GUID? Certainly!!!
                discussionBoardList = web.Lists[discussionBoardTitle];
            }                       

            return discussionBoardList;
        }

        internal static string getWebKeyName(SPList list, string keySuffix)
        {
            return list.ID.ToString() + "_" + keySuffix;
        }

    }
}
