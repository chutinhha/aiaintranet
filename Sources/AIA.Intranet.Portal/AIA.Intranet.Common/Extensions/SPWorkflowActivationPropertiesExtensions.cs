using System;
using System.Web;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPWorkflowActivationPropertiesExtensions
    {
        public static void LogToWorkflowHistory(this SPWorkflowActivationProperties workflowProperties, SPWorkflowHistoryEventType type, SPMember user, string description, string outCome)
        {
            SPWorkflow.CreateHistoryEvent(workflowProperties.Web, workflowProperties.WorkflowId, (int)type, user, TimeSpan.MinValue, outCome, description, string.Empty);
        }

        public static void LogToWorkflowHistory(this SPWorkflowActivationProperties workflowProperties, SPWorkflowHistoryEventType type, string description, string outCome)
        {
            workflowProperties.LogToWorkflowHistory(type, workflowProperties.Site.SystemAccount, description, outCome);
        }

        public static SPListItem GetListItem(this SPWorkflowActivationProperties workflowProperties, string listId, int listItem)
        {
            SPListItem returnItem = null;
            try
            {
                SPList list = workflowProperties.Web.Lists.GetList(new Guid(listId), false);
                returnItem = list.GetItemById(listItem);
            }
            catch { }
            return returnItem;
        }

        public static SPList GetListFromURL(this SPWorkflowActivationProperties workflowProperties, string strURL)
        {
            if (string.IsNullOrEmpty(strURL))
                return null;

            SPSite site = null;
            SPWeb web = null;
            SPList list = null;
            bool disposeSite = false;
            try
            {
                if (CCIUtility.IsAbsoluteUri(strURL))
                    try
                    {
                        site = new SPSite(strURL);
                        web = site.OpenWeb();
                        disposeSite = true;
                    }
                    catch
                    {
                        CCIUtility.LogInfo("Unable to open web from Url : " + strURL + "It isn't SharePoint site or current user don't have permission to open it", "AIA.Intranet");
                    }
                else
                {
                    site = workflowProperties.Site;
                    web = site.OpenWeb(HttpUtility.UrlDecode(strURL), false);
                }

                try
                {
                    list = web.GetList(strURL);
                }
                catch
                {
                    CCIUtility.LogInfo("Unable to load list from Url : " + strURL, "AIA.Intranet");
                }
            }
            catch
            {
                CCIUtility.LogInfo("Couldn't open " + strURL + " as a SharePoint list", "AIA.Intranet");
            }
            finally
            {
                if (web != null) web.Dispose();
                if (disposeSite && site != null) site.Dispose();
            }
            return list;
        }

        public static SPSite GetDestinationSite(this SPWorkflowActivationProperties workflowProperties, string destinationFolderUrl)
        {
            SPSite destinationSite = null;
            if (CCIUtility.IsAbsoluteUri(destinationFolderUrl))
                try
                {
                    //open Site
                    destinationSite = new SPSite(destinationFolderUrl);
                }
                catch { }
            else
                destinationSite = workflowProperties.Site;

            return destinationSite;
        }
    }
}
