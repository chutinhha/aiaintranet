using System;
using System.Runtime.InteropServices;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("e3a1bdda-e183-4ade-befe-ed1ba825bc30")]
    public class PrivateTaskContenTypeEvent : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
            properties.AfterProperties["AssignedTo"] = properties.Web.CurrentUser.ID;
            Employee currentEmployee = EmployeeService.GetEmployeeByUserId(properties.Web.CurrentUser.ID, properties.Web);
            properties.AfterProperties["TaskOwners"] = currentEmployee.ID;
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            try
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(properties.Web.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb(properties.Web.ID))
                            {
                                SPList taskList = web.Lists.TryGetList(properties.ListTitle);
                                if (taskList != null)
                                {
                                    SPListItem item = taskList.GetItemById(properties.ListItemId);
                                    if (item != null)
                                    {
                                        item.BreakRoleInheritance(false);

                                        if (item[SPBuiltInFieldId.AssignedTo] != null)
                                        {
                                            //UpdateListItemModeration(item);
                                            SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                            item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                        }
                                    }
                                }
                            }
                        }
                    });

                    //Update moderation
                    SPModerationInformation moderationInformation = properties.ListItem.ModerationInformation;
                    moderationInformation.Status = SPModerationStatusType.Approved;
                    properties.ListItem.SystemUpdate();

                    //Update Task Statistic
                    TaskService.IncreaseTaskStatistic(properties.Web, properties.Web.CurrentUser.ID, TypeStatistic.Task, Constants.TASK_CONTENT_TYPE_PRIVATE);
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "PrivateTaskContentTypeEvent");
            }
        }

        /// <summary>
        /// An item was updating.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                SPModerationInformation moderationInformation = properties.ListItem.ModerationInformation;
                moderationInformation.Status = SPModerationStatusType.Approved;
                properties.ListItem.SystemUpdate();
            }
        }

        /// <summary>
        /// An item was deleting.
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);
            properties.Web.AllowUnsafeUpdates = true;
            SPFieldUserValueCollection taskOwners = properties.ListItem[SPBuiltInFieldId.AssignedTo] as SPFieldUserValueCollection;
            if (taskOwners != null)
            {
                foreach (var taskOwner in taskOwners)
                {
                    //Update Task Statistic
                    TaskService.DecreaseTaskStatistic(properties.Web, taskOwner.User.ID, TypeStatistic.Task, Constants.TASK_CONTENT_TYPE_PRIVATE);
                }
            }
            properties.Web.AllowUnsafeUpdates = false;
        }

        #region Functions

        private void UpdateListItemModeration(SPListItem item)
        {
            SPModerationInformation moderationInformation = item.ModerationInformation;
            moderationInformation.Status = SPModerationStatusType.Approved;
            item.SystemUpdate();
        }

        private void UpdateListItemModeration(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(properties.Web.ID))
                    {
                        SPList taskList = web.Lists.TryGetList(properties.ListTitle);
                        if (taskList != null)
                        {
                            SPListItem item = taskList.GetItemById(properties.ListItemId);
                            if (item != null)
                            {
                                SPModerationInformation moderationInformation = item.ModerationInformation;
                                moderationInformation.Status = SPModerationStatusType.Approved;
                                item.SystemUpdate();
                            }
                        }
                    }
                }
            });
        }
        #endregion Functions
    }
}
