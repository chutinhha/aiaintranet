using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using AIA.Intranet.Resources;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Recievers
{
    public class ITaskEvent : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);
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

                                        if (item[IOfficeColumnId.Task.TaskManagerUser] != null)
                                        {
                                            SPFieldUserValue taskManager = new SPFieldUserValue(web, Convert.ToString(item[IOfficeColumnId.Task.TaskManagerUser]));
                                            item.SetPermissions(taskManager.User, SPRoleType.Contributor);
                                        }

                                        if (item[SPBuiltInFieldId.AssignedTo] != null)
                                        {
                                            SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                            item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                        }

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
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("ITaskEvent", ex.Message);
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
                                        item.BreakRoleInheritance(true);

                                        if (item[IOfficeColumnId.Task.TaskManagerUser] != null)
                                        {
                                            SPFieldUserValue taskManager = new SPFieldUserValue(web, Convert.ToString(item[IOfficeColumnId.Task.TaskManagerUser]));
                                            item.SetPermissions(taskManager.User, SPRoleType.Contributor);
                                        }

                                        if (item[SPBuiltInFieldId.AssignedTo] != null)
                                        {
                                            SPFieldUserValue taskOwner = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.AssignedTo]));
                                            item.SetPermissions(taskOwner.User, SPRoleType.Contributor);
                                        }

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
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("ITaskEvent", ex.Message);
            }
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }
    }
}
