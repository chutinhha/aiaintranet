using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Common.Controls
{
    public class DepartmentTaskSaveButton : SaveButton
    {
        public string TaskManager { get; set; }

        protected override bool SaveItem()
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            Employee currentUserEmployee = IOfficeContext.CurentUser;
            if (!string.IsNullOrEmpty(TaskManager))
            {
                DropDownList ddlTaskManager = this.TemplateControl.FindControl(TaskManager) as DropDownList;
                if (ddlTaskManager != null && !string.IsNullOrEmpty(ddlTaskManager.SelectedValue))
                {
                    SPFieldLookupValue taskManager = new SPFieldLookupValue(int.Parse(ddlTaskManager.SelectedValue), ddlTaskManager.SelectedItem.Text);
                    CreateTask(int.Parse(ddlTaskManager.SelectedValue), currentUserEmployee.ID, false);
                }
                else
                {
                    CreateTask(currentUserEmployee.ID, currentUserEmployee.ID, true);
                }
            }
            return base.SaveItem();
        }

        #region Functions
        private void EnsuareModeration(string listTitle)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists.TryGetList(listTitle);
                        if (list != null)
                        {
                            list.EnableModeration = true;
                            list.Update();
                        }
                    }
                }
            });
        }

        private void CreateTask(int taskManager, int taskOwnerId, bool isAssigned)
        {
            SPContext.Current.ListItem[IOfficeColumnId.Task.TaskManager] = taskManager;
            Employee taskManagerEmployee = EmployeeService.GetEmployeeByItemId(taskManager);
            SPUser taskManagerUser = SPContext.Current.Web.EnsureUser(taskManagerEmployee.LoginName);
            SPContext.Current.ListItem[IOfficeColumnId.Task.TaskManagerUser] = taskManagerUser;

            SPContext.Current.ListItem[IOfficeColumnId.Task.Assigned] = isAssigned.ToString();

            if (isAssigned)
            {
                FormField ffTaskOwners = this.TemplateControl.FindControl("ffTaskOwners") as FormField;
                SPFieldLookupValueCollection taskOwners = ffTaskOwners.Value as SPFieldLookupValueCollection;
                if (taskOwners != null && taskOwners.Count > 0)
                {
                    SPFieldUserValueCollection taskOwnerColeection = new SPFieldUserValueCollection();
                    foreach (var taskOwner in taskOwners)
                    {
                        Employee temp = EmployeeService.GetEmployeeByItemId(taskOwner.LookupId);
                        SPFieldUserValue fieldUserValue = new SPFieldUserValue(SPContext.Current.Web, temp.UserId, temp.LoginName);
                        taskOwnerColeection.Add(fieldUserValue);
                    }
                    SPContext.Current.ListItem[SPBuiltInFieldId.AssignedTo] = taskOwnerColeection;
                }
            }
            else
            {
                SPContext.Current.ListItem[IOfficeColumnId.Task.TaskOwners] = taskOwnerId;
                Employee taskOwnerEmployee = EmployeeService.GetEmployeeByItemId(taskOwnerId);
                SPUser taskOwnerUser = SPContext.Current.Web.EnsureUser(taskOwnerEmployee.LoginName);
                SPContext.Current.ListItem[SPBuiltInFieldId.AssignedTo] = taskOwnerUser;
            }

            FormField ffTaskSupervisors = this.TemplateControl.FindControl("ffTaskSupervisors") as FormField;
            SPFieldLookupValueCollection taskSupervisors = ffTaskSupervisors.Value as SPFieldLookupValueCollection;
            if (taskSupervisors != null && taskSupervisors.Count > 0)
            {
                SPFieldUserValueCollection taskSupervisorColeection = new SPFieldUserValueCollection();
                foreach (var taskSupervisor in taskSupervisors)
                {
                    Employee temp = EmployeeService.GetEmployeeByItemId(taskSupervisor.LookupId);
                    SPFieldUserValue fieldUserValue = new SPFieldUserValue(SPContext.Current.Web, temp.UserId, temp.LoginName);
                    taskSupervisorColeection.Add(fieldUserValue);
                }
                SPContext.Current.ListItem[IOfficeColumnId.Task.TaskSupervisorUsers] = taskSupervisorColeection;
            }

            FormField ffTaskRelations = this.TemplateControl.FindControl("ffTaskRelations") as FormField;
            SPFieldLookupValueCollection taskRelations = ffTaskRelations.Value as SPFieldLookupValueCollection;
            if (taskRelations != null && taskRelations.Count > 0)
            {
                SPFieldUserValueCollection taskRelationColeection = new SPFieldUserValueCollection();
                foreach (var taskRelation in taskRelations)
                {
                    Employee temp = EmployeeService.GetEmployeeByItemId(taskRelation.LookupId);
                    SPFieldUserValue fieldUserValue = new SPFieldUserValue(SPContext.Current.Web, temp.UserId, temp.LoginName);
                    taskRelationColeection.Add(fieldUserValue);
                }
                SPContext.Current.ListItem[IOfficeColumnId.Task.TaskRelationUsers] = taskRelationColeection;
            }

        }

        //private void CreateTask(int taskManagerId, int taskOwnerId, bool isAssignTask)
        //{
        //    List<SPUser> readers = new List<SPUser>(), contributors = new List<SPUser>();
        //    using (DisableItemEvent disableItemEvent = new DisableItemEvent())
        //    {
        //        SPListItem item = SPContext.Current.List.AddItem();

        //        FormField ffTitle = this.TemplateControl.FindControl("ffTitle") as FormField;
        //        item[SPBuiltInFieldId.Title] = ffTitle.Value;

        //        FormField ffBody = this.TemplateControl.FindControl("ffBody") as FormField;
        //        item[SPBuiltInFieldId.Body] = ffBody.Value;

        //        FormField ffStartDate = this.TemplateControl.FindControl("ffStartDate") as FormField;
        //        item[SPBuiltInFieldId.StartDate] = ffStartDate.Value;

        //        FormField ffDueDate = this.TemplateControl.FindControl("ffDueDate") as FormField;
        //        item[SPBuiltInFieldId.TaskDueDate] = ffDueDate.Value;

        //        FormField ffPriority = this.TemplateControl.FindControl("ffPriority") as FormField;
        //        item[SPBuiltInFieldId.Priority] = ffPriority.Value;

        //        FormField ffStatus = this.TemplateControl.FindControl("ffStatus") as FormField;
        //        item[SPBuiltInFieldId.TaskStatus] = ffStatus.Value;

        //        FormField ffPercentComplete = this.TemplateControl.FindControl("ffPercentComplete") as FormField;
        //        item[SPBuiltInFieldId.PercentComplete] = ffPercentComplete.Value;

        //        FormField ffActualStart = this.TemplateControl.FindControl("ffActualStart") as FormField;
        //        item[IOfficeColumnId.Task.ActualStart] = ffActualStart.Value;

        //        FormField ffActualEnd = this.TemplateControl.FindControl("ffActualEnd") as FormField;
        //        item[IOfficeColumnId.Task.ActualEnd] = ffActualEnd.Value;

        //        item[IOfficeColumnId.Task.TaskManager] = taskManagerId;
        //        Employee taskManagerEmployee = EmployeeService.GetEmployeeByItemId(taskManagerId);
        //        SPUser taskManager = SPContext.Current.Web.EnsureUser(taskManagerEmployee.LoginName);
        //        contributors.Add(taskManager);
        //        item[IOfficeColumnId.Task.TaskManagerUser] = taskManager;

        //        item[IOfficeColumnId.Task.TaskOwners] = taskOwnerId;
        //        Employee taskOwnerEmployee = EmployeeService.GetEmployeeByItemId(taskOwnerId);
        //        SPUser taskOwner = SPContext.Current.Web.EnsureUser(taskOwnerEmployee.LoginName);
        //        item[SPBuiltInFieldId.AssignedTo] = taskOwner;
        //        contributors.Add(taskOwner);

        //        FormField ffTaskSupervisors = this.TemplateControl.FindControl("ffTaskSupervisors") as FormField;
        //        item[IOfficeColumnId.Task.TaskSupervisors] = ffTaskSupervisors.Value;
        //        SPFieldLookupValueCollection taskSupervisors = ffTaskSupervisors.Value as SPFieldLookupValueCollection;
        //        if (taskSupervisors != null)
        //        {
        //            SPFieldUserValueCollection taskSupervisorColeection = new SPFieldUserValueCollection();
        //            foreach (var taskSupervisor in taskSupervisors)
        //            {
        //                Employee temp = EmployeeService.GetEmployeeByItemId(taskSupervisor.LookupId);
        //                SPFieldUserValue fieldUserValue = new SPFieldUserValue(SPContext.Current.Web, temp.UserId, temp.LoginName);
        //                readers.Add(fieldUserValue.User);
        //                taskSupervisorColeection.Add(fieldUserValue);
        //            }
        //            item[IOfficeColumnId.Task.TaskSupervisorUsers] = taskSupervisorColeection;
        //        }

        //        FormField ffTaskRelations = this.TemplateControl.FindControl("ffTaskRelations") as FormField;
        //        item[IOfficeColumnId.Task.TaskRelations] = ffTaskRelations.Value;
        //        SPFieldLookupValueCollection taskRelations = ffTaskRelations.Value as SPFieldLookupValueCollection;
        //        if (taskRelations != null)
        //        {
        //            SPFieldUserValueCollection taskRelationColeection = new SPFieldUserValueCollection();
        //            foreach (var taskRelation in taskRelations)
        //            {
        //                Employee temp = EmployeeService.GetEmployeeByItemId(taskRelation.LookupId);
        //                SPFieldUserValue fieldUserValue = new SPFieldUserValue(SPContext.Current.Web, temp.UserId, temp.LoginName);
        //                readers.Add(fieldUserValue.User);
        //                taskRelationColeection.Add(fieldUserValue);
        //            }
        //            item[IOfficeColumnId.Task.TaskRelationUsers] = taskRelationColeection;
        //        }
        //        item.Update();
        //        UpdateItemPermission(item.ID, readers, contributors, isAssignTask);
        //    }
        //}

        //private void UpdateItemPermission(int itemId, List<SPUser> readers, List<SPUser> contributors, bool isAssignTask)
        //{
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                SPList list = web.Lists.TryGetList(SPContext.Current.List.Title);
        //                if (list != null)
        //                {
        //                    SPListItem item = list.GetItemById(itemId);
        //                    if (item != null)
        //                    {
        //                        item.BreakRoleInheritance(false);

        //                        if (readers != null && readers.Count > 0)
        //                        {
        //                            foreach (var user in readers)
        //                            {
        //                                item.SetPermissions(user, SPRoleType.Reader);
        //                            }
        //                        }

        //                        if (contributors != null && contributors.Count > 0)
        //                        {
        //                            foreach (var user in contributors)
        //                            {
        //                                item.SetPermissions(user, SPRoleType.Contributor);
        //                            }
        //                        }

        //                        //item.SetCustomSettings(IOfficeFeatures.Infrastructure, setting);
        //                    }
        //                }
        //            }
        //        }
        //    });
        //}

        #endregion Functions
    }
}
