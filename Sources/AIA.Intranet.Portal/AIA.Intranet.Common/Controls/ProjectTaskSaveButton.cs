using System.Web.UI.WebControls;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Common.Controls
{
    public class ProjectTaskSaveButton : SaveButton
    {
        public string TaskOnProject { get; set; }

        protected override bool SaveItem()
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            Employee currentUserEmployee = IOfficeContext.CurentUser;
            if (!string.IsNullOrEmpty(TaskOnProject))
            {
                DropDownList ddlTaskOnProject = this.TemplateControl.FindControl(TaskOnProject) as DropDownList;
                if (ddlTaskOnProject != null && !string.IsNullOrEmpty(ddlTaskOnProject.SelectedValue))
                {
                    CreateProjectTask(int.Parse(ddlTaskOnProject.SelectedValue), currentUserEmployee.ID, false);
                }
            }

            return base.SaveItem();
        }

        private void CreateProjectTask(int projectID, int taskOwnerID, bool isAssigned)
        {
            SPListItem currentItem = SPContext.Current.ListItem;

            currentItem[IOfficeColumnId.Task.TaskOnProject] = projectID;

            ProjectItem project = ProjectServices.GetProjectByItemID(projectID);
            currentItem[IOfficeColumnId.Task.TaskManager] = project.ProjectManagerID;
            Employee projectManager = EmployeeService.GetEmployeeByItemId(project.ProjectManagerID);
            SPUser taskManager = SPContext.Current.Web.EnsureUser(projectManager.LoginName);
            currentItem[IOfficeColumnId.Task.TaskManagerUser] = taskManager.ID;

            currentItem[IOfficeColumnId.Task.Assigned] = isAssigned.ToString();

            if (project.ProjectClerkID != 0)
            {
                currentItem[IOfficeColumnId.Task.TaskSupervisors] = project.ProjectClerkID;
                Employee projectClerk = EmployeeService.GetEmployeeByItemId(project.ProjectClerkID);
                SPUser taskSupervisor = SPContext.Current.Web.EnsureUser(projectClerk.LoginName);
                currentItem[IOfficeColumnId.Task.TaskSupervisorUsers] = taskSupervisor.ID;
            }

            currentItem[IOfficeColumnId.Task.TaskOwners] = taskOwnerID;
            currentItem[SPBuiltInFieldId.AssignedTo] = SPContext.Current.Web.CurrentUser.ID;
        }
    }
}
