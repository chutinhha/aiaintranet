using System;
using System.Runtime.InteropServices;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using System.Collections.Generic;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Model;
using AIA.Intranet.Resources;
using System.Collections;

namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("e3a1abdd-e183-dea4-bcfe-ed1a8b25bc30")]
    public class DepartmentTaskContentTypeEvent : SPItemEventReceiver
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
            try
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    UpdateListItemPermission(properties);
                    if (properties.ListItem[IOfficeColumnId.Task.Assigned] != null && Convert.ToBoolean(properties.ListItem[IOfficeColumnId.Task.Assigned].ToString()))
                    {
                        SPModerationInformation moderationInformation = properties.ListItem.ModerationInformation;
                        moderationInformation.Status = SPModerationStatusType.Approved;
                        properties.ListItem.SystemUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("DepartmentTaskContentTypeEvent", ex.Message);
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
                        SPModerationInformation moderationInformation = properties.ListItem.ModerationInformation;
                        moderationInformation.Status = SPModerationStatusType.Approved;
                        properties.ListItem.SystemUpdate();
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

        #region Functions
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

        private void UpdateItemPermission(int itemId, List<SPUser> readers, List<SPUser> contributors)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists.TryGetList(SPContext.Current.List.Title);
                        if (list != null)
                        {
                            SPListItem item = list.GetItemById(itemId);
                            if (item != null)
                            {
                                item.BreakRoleInheritance(false);

                                if (readers != null && readers.Count > 0)
                                {
                                    foreach (var user in readers)
                                    {
                                        item.SetPermissions(user, SPRoleType.Reader);
                                    }
                                }

                                if (contributors != null && contributors.Count > 0)
                                {
                                    foreach (var user in contributors)
                                    {
                                        item.SetPermissions(user, SPRoleType.Contributor);
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private void UpdateListItemPermission(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(properties.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList taskList = web.Lists.TryGetList(properties.ListTitle);
                        if (taskList != null)
                        {
                            SPListItem item = taskList.GetItemById(properties.ListItemId);
                            if (item != null)
                            {
                                item.BreakRoleInheritance(false);

                                if (item[IOfficeColumnId.Task.TaskManagerUser] != null)
                                {
                                    SPFieldUserValue taskManager = new SPFieldUserValue(web, Convert.ToString(item[IOfficeColumnId.Task.TaskManagerUser]));
                                    item.SetPermissions(taskManager.User, SPRoleType.Contributor);
                                }

                                SPFieldUserValueCollection taskOwners = item[SPBuiltInFieldId.AssignedTo] as SPFieldUserValueCollection;
                                if (taskOwners != null)
                                {
                                    foreach (var taskOwner in taskOwners)
                                    {
                                        item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                        //Update Task Statistic
                                        TaskService.IncreaseTaskStatistic(web, taskOwner.User.ID, TypeStatistic.Task, Constants.TASK_CONTENT_TYPE_DEPARTMENT);
                                    }
                                }
                                //if (item[SPBuiltInFieldId.AssignedTo] != null)
                                //{
                                    //SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                    //item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                //}

                                SPFieldUserValueCollection taskSupervisors = item[IOfficeColumnId.Task.TaskSupervisorUsers] as SPFieldUserValueCollection;
                                if (taskSupervisors != null)
                                {
                                    foreach (var taskSupervisor in taskSupervisors)
                                    {
                                        item.SetPermissions(taskSupervisor.User, SPRoleType.Reader);
                                    }
                                }

                                SPFieldUserValueCollection taskRelations = item[IOfficeColumnId.Task.TaskRelationUsers] as SPFieldUserValueCollection;
                                if (taskRelations != null)
                                {
                                    foreach (var taskRelation in taskRelations)
                                    {
                                        item.SetPermissions(taskRelation.User, SPRoleType.Reader);
                                    }
                                }
                            }
                            web.AllowUnsafeUpdates = false;
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
                                    SPFieldUserValueCollection taskOwners = item[SPBuiltInFieldId.AssignedTo] as SPFieldUserValueCollection;
                                    if (taskOwners != null)
                                    {
                                        foreach (var taskOwner in taskOwners)
                                        {
                                            item.ChangePermissions(taskOwner.User, SPRoleType.Reader);
                                        }
                                    }
                                    //if (item[SPBuiltInFieldId.AssignedTo] != null)
                                    //{
                                    //    SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                    //    item.ChangePermissions(taskOwner.User, SPRoleType.Reader);
                                    //}
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
        #endregion Functions
    }
}
