using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class TaskItem : BaseEntity
    {
        public TaskItem(SPListItem item)
            : base(item)
        {
            try
            {
                ContentTypeId = item.ContentTypeId.ToString();

                int currentUser = SPContext.Current.Web.CurrentUser.ID;
                SPWeb web = SPContext.Current.Web;
                //Task Manager
                IsTaskManager = false;
                if (!string.IsNullOrEmpty(TaskManagerUser))
                {
                    SPFieldUserValue taskManager = new SPFieldUserValue(web, TaskManagerUser);
                    if (currentUser == taskManager.User.ID)
                        IsTaskManager = true;
                }
                //Task Supervisor
                IsTaskSupervisor = false;
                if (!string.IsNullOrEmpty(TaskSupervisorUsers))
                {
                    SPFieldUserValueCollection taskSupervisors = new SPFieldUserValueCollection(web, TaskSupervisorUsers);
                    if (taskSupervisors != null && taskSupervisors.Count > 0)
                    {
                        foreach (var taskSupervisor in taskSupervisors)
                        {
                            if (currentUser == taskSupervisor.User.ID)
                            {
                                IsTaskSupervisor = true;
                                break;
                            }
                        }
                    }
                }
                //Task Owner
                IsTaskOwner = false;
                var assignedTo = item[SPBuiltInFieldId.AssignedTo];
                if (assignedTo != null && !string.IsNullOrEmpty(assignedTo.ToString()))
                {
                    SPFieldUserValueCollection taskOwners = new SPFieldUserValueCollection(web, assignedTo.ToString());
                    if (taskOwners != null && taskOwners.Count > 0)
                    {
                        foreach (var taskOwner in taskOwners)
                        {
                            if (currentUser == taskOwner.User.ID)
                            {
                                IsTaskOwner = true;
                                break;
                            }
                        }
                    }
                }
                //Task Approve
                IsTaskApprove = false;
                if (this.ApprovalStatus == Constants.APPROVED)
                {
                    IsTaskApprove = true;
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public string ContentTypeId { get; set; }

        public string TaskManagerUser { get; set; }
        public string TaskSupervisorUsers { get; set; }
        public string Assigned { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ActualStart { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime ActualEnd { get; set; }

        [Field(Ignore = true)]
        public bool IsTaskManager { get; set; }

        [Field(Ignore = true)]
        public bool IsTaskOwner { get; set; }

        [Field(Ignore = true)]
        public bool IsTaskSupervisor { get; set; }

        [Field(Ignore = true)]
        public bool IsTaskApprove { get; set; }

        public string AssignedTo { get; set; }
    }
}
