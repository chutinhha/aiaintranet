using System;
using System.Runtime.InteropServices;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using System.Collections;
using AIA.Intranet.Resources;
using AIA.Intranet.Common.Services;

namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("ae31ae2d-e183-dea4-bcfe-ed1a8b250bc3")]
    public class ProjectTaskContentTypeEvent : SPItemEventReceiver
    {
        private static Hashtable ApprovalStatus = new Hashtable();

        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                UpdateListItemPermission(properties);
            }
        }

        /// <summary>
        /// An item was updating.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            if (properties.ListItem[SPBuiltInFieldId._ModerationStatus] != null)
            {
                ApprovalStatus[properties.ListItemId] = properties.ListItem[SPBuiltInFieldId._ModerationStatus].ToString();
            }
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            try
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    if (ApprovalStatus[properties.ListItemId].ToString() == Constants.APPROVED)
                    {
                        UpdateListItemModeration(properties);
                        ApprovalStatus.Remove(properties.ListItemId);
                    }
                    else
                    {
                        UpdateListItemPermission(properties);
                    }
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("DepartmentTaskContentTypeEvent", ex.Message);
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
                    TaskService.DecreaseTaskStatistic(properties.Web, taskOwner.User.ID, TypeStatistic.Task, Constants.TASK_CONTENT_TYPE_DEPARTMENT);
                }
            }
            properties.Web.AllowUnsafeUpdates = false;
        }

        private static void UpdateListItemModeration(SPItemEventProperties properties)
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

        private void ChangeListTemPermission(SPItemEventProperties properties)
        {
            try
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
                                    if (item[SPBuiltInFieldId.AssignedTo] != null)
                                    {
                                        SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                        item.ChangePermissions(taskOwner.User, SPRoleType.Reader);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "DepartmentTaskContentTypeEvent");
            }
        }

        private void UpdateListItemPermission(SPItemEventProperties properties)
        {
            try
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
                                        SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                        item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                        //Update Task Statistic
                                        TaskService.IncreaseTaskStatistic(web, taskOwner.User.ID, TypeStatistic.Task, Constants.TASK_CONTENT_TYPE_PROJECT);
                                    }

                                    if (item[IOfficeColumnId.Task.TaskManagerUser] != null)
                                    {
                                        SPFieldUserValue taskManager = new SPFieldUserValue(web, Convert.ToString(item[IOfficeColumnId.Task.TaskManagerUser]));
                                        item.SetPermissions(taskManager.User, SPRoleType.Contributor);
                                    }

                                    if (item[IOfficeColumnId.Task.TaskSupervisors] != null)
                                    {
                                        SPFieldUserValueCollection taskSupervisors = new SPFieldUserValueCollection(web, Convert.ToString(item[IOfficeColumnId.Task.TaskSupervisorUsers]));
                                        if (taskSupervisors != null && taskSupervisors.Count > 0)
                                        {
                                            foreach (var taskSupervisor in taskSupervisors)
                                            {
                                                item.SetPermissions(taskSupervisor.User, SPRoleType.Contributor);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "DepartmentTaskContentTypeEvent");
            }
        }
    }
}
