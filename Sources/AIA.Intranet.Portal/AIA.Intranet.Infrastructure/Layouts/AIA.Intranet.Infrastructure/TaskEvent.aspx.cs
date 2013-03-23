using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Infrastructure.Controls;
using AIA.Intranet.Model.Workflow;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class TaskEvent : LayoutsPageBase
    {
        [Serializable]
        public class DynamidControlPlaceHolder
        {
            public string Id { get; set; }
            public string VirtualPath { get; set; }
        };
       
        List<DynamidControlPlaceHolder> _controls;
        List<ActionEditorControl> _actionControls;

        private void initilizeControls()
        {
            _actionControls = new List<ActionEditorControl>();
            if (ViewState["_ACTION_CONTROLS"] != null)
            {
                _controls = ViewState["_ACTION_CONTROLS"] as List<DynamidControlPlaceHolder>;
            }
            else
            {
                _controls = new List<DynamidControlPlaceHolder>();
            }
            foreach (DynamidControlPlaceHolder holder in _controls)
            {
                Control ctrl = Page.LoadControl(holder.VirtualPath);
                ctrl.ID = holder.Id;
                _actionControls.Add(ctrl as ActionEditorControl);
                actionControlsPlaceHolder.Controls.Add(ctrl);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            lnkAddAction.Click += new EventHandler(lnkAddAction_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            base.OnInit(e);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            List<TaskActionSettings> actions = new List<TaskActionSettings>();
            foreach (ActionEditorControl ctrl in _actionControls)
            {
                TaskActionSettings setting = ctrl.GetAction();
                if (setting != null)
                {
                    actions.Add(ctrl.GetAction());
                }
            }
            TaskEventSetting editEvent = getEditingTaskEvent();
            editEvent.Actions = actions;
            CallbackAndClose();
        }

        private void CallbackAndClose()
        {
            //Close popup
            Context.Response.Clear();
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();

            //string script = string.Empty;
            //script = "<script language='javascript'>";
            //script += "    function CallbackAndClose(){";
            //script += "        setModalDialogReturnValue(parent.window, '');\r\n                   ";
            //script += "        parent.window.close();\r\n   ;";
            //script += "    }";
            //script += "    if (window.addEventListener){";
            //script += "        window.addEventListener('load', CallbackAndClose, false);";
            //script += "    }";
            //script += "    else if (window.attachEvent){";
            //script += "        window.attachEvent('onload', CallbackAndClose)";
            //script += "    }";
            //script += "</script>";
            //this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "CallbackAndClose", script);
        }

        private TaskEventSetting getEditingTaskEvent()
        {
            string session = Request["session"] as string;
            TaskEventSettings settings = Session[session] as TaskEventSettings;
            if (settings == null)
            {
                settings = new TaskEventSettings();
            }

            TaskEventTypes editType = (TaskEventTypes)Enum.Parse(typeof(TaskEventTypes), Request["type"] as string);
            TaskEventSetting editEvent = settings.Events.Where(p => p.Type == editType).SingleOrDefault();
            if (editEvent == null)
            {
                editEvent = new TaskEventSetting()
                {
                    Type = editType,
                };
                settings.Events.Add(editEvent);
                Session[session] = settings;
            }
            return editEvent;
        }

        void lnkAddAction_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlActionTypes.SelectedValue)) return;
            TaskActionTypes type = (TaskActionTypes)Enum.Parse(typeof(TaskActionTypes), ddlActionTypes.SelectedValue);

            DynamidControlPlaceHolder holder = new DynamidControlPlaceHolder()
            {
                Id = "ActionControlID_" + _controls.Count.ToString(),
                VirtualPath = getControlPath(type)
            };
            _controls.Add(holder);
            ViewState["_ACTION_CONTROLS"] = _controls;
            ActionEditorControl ctrl = Page.LoadControl(holder.VirtualPath) as ActionEditorControl;
            ctrl.ID = holder.Id;
            ctrl.IsFirstLoad = true;
            ctrl.ListId = Request["List"];
            ctrl.ContentTypeId = Request["ctype"];
            actionControlsPlaceHolder.Controls.Add(ctrl);
        }

        protected override void OnLoad(EventArgs e)
        {
            initilizeControls();

            if (!IsPostBack)
            {

                loadControlFromSession();
                displayActionTypes();

                bool isReadOnly = bool.Parse(Request["readonly"]);
                if (isReadOnly)
                {
                    LockUI();
                }
            }
            base.OnLoad(e);
        }

        private void LockUI()
        {
            foreach (ActionEditorControl ctrl in _actionControls)
            {
                ctrl.ReadOnly = true;
            }
            ddlActionTypes.Enabled = false;
            lnkAddAction.Enabled = false;
            btnSave.Visible = false;
        }

        private void loadControlFromSession()
        {
            _controls = new List<DynamidControlPlaceHolder>();
            _actionControls = new List<ActionEditorControl>();
            TaskEventSetting editingEvent = getEditingTaskEvent();
            if (editingEvent != null && editingEvent.Actions.Count > 0)
            {
                foreach (TaskActionSettings action in editingEvent.Actions)
                {
                    DynamidControlPlaceHolder holder = new DynamidControlPlaceHolder()
                    {
                        Id = "ActionControlID_" + _controls.Count.ToString(),

                        VirtualPath = getControlPath(action)
                    };
                    _controls.Add(holder);
                    ActionEditorControl ctrl = Page.LoadControl(holder.VirtualPath) as ActionEditorControl;
                    ctrl.Action = action;
                    ctrl.ID = holder.Id;
                    _actionControls.Add(ctrl);
                    actionControlsPlaceHolder.Controls.Add(ctrl);
                }
                ViewState["_ACTION_CONTROLS"] = _controls;
            }
        }

        private string getControlPath(TaskActionTypes type)
        {
            string vitualPath = string.Empty;
            switch (type)
            {
                case TaskActionTypes.SendEmailToStaticAddresses:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailToStaticAddressEditor.ascx";
                    break;
                case TaskActionTypes.SendEmailToWorkflowItemUserColumn:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailToWfItemUserColumnEditor.ascx";
                    break;
                case TaskActionTypes.SendEmailToWorkflowTaskUserColumn:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailtoWorkflowTaskUserColumnEditor.ascx";
                    break;
                case TaskActionTypes.UpdateWorkflowItemMetadata:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowItemMetaDataEditor.ascx";
                    break;
                case TaskActionTypes.UpdateWorkflowTaskMetadata:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowTaskMetaDataEditor.ascx";
                    break;
                case TaskActionTypes.SendEmailWithESignMetadataToStaticAddresses:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailWithESignMetadataToStaticAddressEditor.ascx";
                    break;

                case TaskActionTypes.SendEmailWithESignMetadataToWorkflowItemUserColumn:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailWithESignMetadataToWfItemUserColumnEditor.ascx";
                    break;
                case TaskActionTypes.UpdateWorkflowItemWithEsignVariables:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowItemWithEsignVariablesEditor.ascx";
                    break;
                case TaskActionTypes.UpdateWorkflowItemWithESignMetadata:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowItemWithESignMetaDataEditor.ascx";
                    break;
                case TaskActionTypes.SendEmailWithESignVariableToStaticAddresses:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailWithRecipientVariableToStaticAddressEditor.ascx";
                    break;

                case TaskActionTypes.SendEmailWithESignVariableToWfItemUserColumn:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/SendEmailWithESignVariableToWfItemUserColumnEditor.ascx";
                    break;
                case TaskActionTypes.UpdateExecutedDocumentMetadata:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateExecutedDocumentMetaDataEditor.ascx";
                    break;

                case TaskActionTypes.UpdateWorkflowItemWithKeyword:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowItemWithKeywordEditor.ascx";
                    break;
                case TaskActionTypes.UpdateWorkflowItemWithTaskProperty:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWorkflowItemWithTaskPropertyEditor.ascx";
                    break;

                case TaskActionTypes.UpdateTaskItemWithItemProperty:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateTaskItemWithItemPropertyEditor.ascx";
                    break;

                case TaskActionTypes.UpdateWFPermission:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateWFItemPermissionEditor.ascx";
                    break;

                case TaskActionTypes.CreateUnreadTask:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/CreateUnreadTaskEditor.ascx";
                    break;
                     case TaskActionTypes.UpdateTaskPermission:
                    vitualPath = "~/_controltemplates/AIA.Intranet.Infrastructure/ActionEditors/UpdateTaskPermissionEditor.ascx";
                    break;


                default:
                    return string.Empty;
            }
            return vitualPath;

        }

        private string getControlPath(TaskActionSettings action)
        {
            return getControlPath(action.Type);
        }

        private void displayActionTypes()
        {
            ddlActionTypes.Items.Clear();
            ddlActionTypes.Items.Add(new ListItem() { Value = "", Text = "Select an action" });
            string eventOwner = Request["eventOwner"];

            EventOwners owner = (EventOwners)Enum.Parse(typeof(EventOwners), eventOwner);
            List<ListItem> listItems = null;

            switch (owner)
            {
                case EventOwners.DocuSignProcess:
                    listItems = getDocuSignProcessActions();
                    break;
                case EventOwners.Workflow:
                    listItems = getWorkflowActions();
                    break;
                case EventOwners.ApprovalWF:
                    listItems = getWorkflowActions();
                    
                    break;
            }
            ddlActionTypes.Items.AddRange(listItems.ToArray());

        }

        private List<ListItem> getWorkflowActions()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem() { Value = TaskActionTypes.SendEmailToStaticAddresses.ToString(), Text = "Send email to static addresses" });
            items.Add(new ListItem() { Value = TaskActionTypes.SendEmailToWorkflowItemUserColumn.ToString(), Text = "Send email to workflow item user column" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemMetadata.ToString(), Text = "Update metadata of workflow item" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemWithTaskProperty.ToString(), Text = "Update Item Property With Task Property" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemWithKeyword.ToString(), Text = "Update metadata of workflow item with keywords" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWFPermission.ToString(), Text = "Update item permissions" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateTaskPermission.ToString(), Text = "Update task permissions" });
            items.Add(new ListItem() { Value = TaskActionTypes.CreateUnreadTask.ToString(), Text = "Create unread task" });
            TaskEventTypes editType = (TaskEventTypes)Enum.Parse(typeof(TaskEventTypes), Request["type"] as string);
            //not create task, so no task to do
            if (editType != TaskEventTypes.ByPassTask)
            {
                items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowTaskMetadata.ToString(), Text = "Update Metadata of Task" });
                items.Add(new ListItem() { Value = TaskActionTypes.UpdateTaskItemWithItemProperty.ToString(), Text = "Update Task Property With Item Property" });
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailToWorkflowTaskUserColumn.ToString(), Text = "Send email to Task User Column" });
            }

            return items;
        }

        private List<ListItem> getDocuSignProcessActions()
        {
            TaskEventTypes currentType = (TaskEventTypes)Enum.Parse(typeof(TaskEventTypes), Request["type"] as string);

            List<ListItem> items = new List<ListItem>();
           
            if (currentType != TaskEventTypes.DocumentSent)
            {
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailWithESignMetadataToStaticAddresses.ToString(), Text = "Send email with ESignMetadata to static addresses" });
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailWithESignMetadataToWorkflowItemUserColumn.ToString(), Text = "Send email with ESignMetadata to workflow item user column" });
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailWithESignVariableToStaticAddresses.ToString(), Text = "Send email with ESign variable to static addresses" });
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailWithESignVariableToWfItemUserColumn.ToString(), Text = "Send email with ESign variable to workflow item user column" });
                items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemWithESignMetadata.ToString(), Text = "Update workflow item with ESignMetadata" });
            }
            else
            {
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailToStaticAddresses.ToString(), Text = "Send email to static addresses" });
                items.Add(new ListItem() { Value = TaskActionTypes.SendEmailToWorkflowItemUserColumn.ToString(), Text = "Send email to workflow item user column" });
                //items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemMetadata.ToString(), Text = "Update metadata of workflow item" });
            }

            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemMetadata.ToString(), Text = "Update metadata of workflow item" });
            items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemWithKeyword.ToString(), Text = "Update metadata of workflow item with keywords" });
          
            TaskEventTypes editType = (TaskEventTypes)Enum.Parse(typeof(TaskEventTypes), Request["type"] as string);
            if (editType == TaskEventTypes.DocumentRetrieved)
                items.Add(new ListItem() { Value = TaskActionTypes.UpdateExecutedDocumentMetadata.ToString(), Text = "Update Metadata of Executed Document" });

            if (!string.IsNullOrEmpty(Request["listSettings"]) && currentType != TaskEventTypes.DocumentSent)
            {
                items.Add(new ListItem() { Value = TaskActionTypes.UpdateWorkflowItemWithEsignVariables.ToString(), Text = "Update workflow item with ESign variable" });
            }
            return items;
        }
    }
}
