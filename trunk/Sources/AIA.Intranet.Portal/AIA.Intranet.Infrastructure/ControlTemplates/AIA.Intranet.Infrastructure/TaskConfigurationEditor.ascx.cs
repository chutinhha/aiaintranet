using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
//using AIA.Intranet.Model.Search;
//using AIA.Intranet.Model.Workflow;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Model.Workflow;
using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class TaskConfigurationEditor : UserControl
    {
        protected string ApprovedSessionKey { get; set; }
        protected string RejectedSessionKey { get; set; }
        protected string TerminationSessionKey { get; set; }
        protected string ReassignSessionKey { get; set; }
        protected string RequestInformationSessionKey { get; set; }
        protected string FinishedSessionKey { get; set; }
        protected string SignatureVerificationSessionKey { get; set; }
        protected string DataQualityCompletedSessionKey { get; set; }
        protected bool FormReadOnly = false;

        protected bool IsDialog
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["IsDlg"]))
                    return false;
                try
                {
                    return Convert.ToBoolean(Convert.ToByte(Request.QueryString["IsDlg"].Split(',')[0]));
                }
                catch { return false; }
            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }  

        protected override void OnInit(EventArgs e)
        {
            bntVirtualButton.Click += new EventHandler(bntVirtualButton_Click);
            RefeshButton.Click += new EventHandler(RefeshButton_Click);
            ReassignCheckBox.Attributes.Add("onclick", string.Format("handleDependentCheckBox(this,'{0}');", AllowDueDateChangeRessignmentCheckBox.ClientID));
            RequestInfomationCheckBox.Attributes.Add("onclick", string.Format("handleDependentCheckBox(this,'{0}');", AllowDueDateChangeRequestInfomationCheckBox.ClientID));
            txtTemplateUrl.TextChanged+=new EventHandler(txtTemplateUrl_TextChanged);
            base.OnInit(e);
        }

        void txtTemplateUrl_TextChanged(object sender, EventArgs e)
        {
            PopulateEmailTemplateDropDowns();
        }

       
   
        void bntVirtualButton_Click(object sender, EventArgs e)
        {
            string selectedValue = TaskContentTypesDropDown.SelectedValue;
            List<SPContentType> contentTypes = SPContext.Current.Web.FindAllContentTypesOf(SPBuiltInContentTypeId.WorkflowTask);
            TaskContentTypesDropDown.DataSource = contentTypes;
            TaskContentTypesDropDown.DataTextField = "Name";
            TaskContentTypesDropDown.DataValueField = "Id";
            TaskContentTypesDropDown.DataBind();
            TaskContentTypesDropDown.SelectedValue = selectedValue;
        }

        protected void RefeshButton_Click(object sender, EventArgs e)
        {
            TaskEventSettings settings = Session[_hiddenUniqueKey.Value] as TaskEventSettings;
            if (settings != null)
            {
                ChangeLabelEvent(settings);
            }
        }

        protected void ParticipantsRadioChange(object sender, EventArgs e)
        {
            showApproversSection();
            if (UseMetadataAssignmentRadio.Checked)
            {
                ApproversDropDown.SelectedIndex = 0;
                ApproversPeoplePicker.AllowEmpty = true;
                ApproversPeoplePicker.ValidatorEnabled = false;
            }
            else
            {
                ApproversPeoplePicker.Entities.Clear();
                ApproversPeoplePicker.AllowEmpty = false;
                ApproversPeoplePicker.ValidatorEnabled = true;
            }
        }

        private void showApproversSection()
        {
            ApproversDropDownDiv.Visible = UseMetadataAssignmentRadio.Checked;
            ApproversPeoplePickerDiv.Visible = SpecifiedParticipantsRadio.Checked;
        }

        protected void ByPassTaskCheckedChanged(object sender, EventArgs e)
        {
            ByPassTaskDiv.Visible = ByPassTaskCheckBox.Checked;
        }

        protected void TaskContentTypesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[ApprovedSessionKey] = new List<Criteria>();
            Session[RejectedSessionKey] = new List<Criteria>();
            Session[TerminationSessionKey] = new List<Criteria>();
            Session[ReassignSessionKey] = new List<Criteria>();
            Session[RequestInformationSessionKey] = new List<Criteria>();
            Session[FinishedSessionKey] = new List<Criteria>();
            Session[SignatureVerificationSessionKey] = new List<Criteria>();
            Session[DataQualityCompletedSessionKey] = new List<Criteria>();

            string ContentTypeId = TaskContentTypesDropDown.SelectedValue.ToLower().Trim();
            switchContentDisplay(ContentTypeId);
        }

        private void switchContentDisplay(string ContentTypeId)
        {
            string parentContentType = getParentContentType(ContentTypeId);
            switch (parentContentType)
            {
                case Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID:
                    {
                        SignatureVerificationRuleErrorLabel.Visible = true;
                        SignatureVerificationTaskRuleDiv.Visible = true;
                        DataQualityCompletedTaskRuleDiv.Visible = false;
                        DataQualityCompletedRuleErrorLabel.Visible = false;
                        AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = false;
                        WorkflowTaskSettingTable.Visible = true;
                        ApprovedRuleErrorLabel.Visible = false;
                        CCIWorkflowTaskSettingDiv.Visible = WorkflowTaskExSettingTable.Visible = false;
                        break;
                    }                
                case Constants.DATA_QUALITY_CONTENT_TYPE_ID:
                    {
                        SignatureVerificationTaskRuleDiv.Visible = false;
                        SignatureVerificationRuleErrorLabel.Visible = false;
                        AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = false;                        
                        DataQualityCompletedTaskRuleDiv.Visible = true;
                        ApprovedRuleErrorLabel.Visible = false;
                        DataQualityCompletedRuleErrorLabel.Visible = true;
                        WorkflowTaskSettingTable.Visible = CCIWorkflowTaskSettingDiv.Visible = WorkflowTaskExSettingTable.Visible = false;                        
                        break;
                    }
                default:
                    {
                        SignatureVerificationTaskRuleDiv.Visible = false;
                        SignatureVerificationRuleErrorLabel.Visible = false;
                        DataQualityCompletedTaskRuleDiv.Visible = false;
                        DataQualityCompletedRuleErrorLabel.Visible = false;
                        ApprovedRuleErrorLabel.Visible = true;
                        AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = true;
                        CCIWorkflowTaskSettingDiv.Visible = true;
                        WorkflowTaskSettingTable.Visible = WorkflowTaskExSettingTable.Visible = false;
                        if (string.Compare(parentContentType, Constants.CCI_WORKFLOW_TASK_CONTENT_TYPE_ID, true) == 0)
                        {   
                            WorkflowTaskSettingTable.Visible = true;
                            WorkflowTaskExSettingTable.Visible = true;
                        }
                        break;
                    }
            }
        }

        private string getParentContentType(string ContentTypeId)
        {
            SPContentTypeId cciappWorkflowTaskCT = new SPContentTypeId(Constants.CCI_WORKFLOW_TASK_ID);
            SPContentTypeId cciappWorkflowSignatureCT = new SPContentTypeId(Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID);
            SPContentTypeId cciappDataQualityCT = new SPContentTypeId(Constants.DATA_QUALITY_CONTENT_TYPE_ID);
            SPContentTypeId selectedCT = new SPContentTypeId(ContentTypeId);

            if (selectedCT.IsChildOf(cciappDataQualityCT))
                return cciappDataQualityCT.ToString().ToLower();

            if (selectedCT.IsChildOf(cciappWorkflowSignatureCT))
                return cciappWorkflowSignatureCT.ToString().ToLower();

            if (selectedCT.IsChildOf(cciappWorkflowTaskCT))
                return cciappWorkflowTaskCT.ToString().ToLower();

            return selectedCT.ToString().ToLower();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreloadData();
                ApprovedSessionKeyTextBox.Text = ApprovedSessionKey = Guid.NewGuid().ToString();
                RejectedSessionKeyTextBox.Text = RejectedSessionKey = Guid.NewGuid().ToString();
                TerminationSessionKeyTextBox.Text = TerminationSessionKey = Guid.NewGuid().ToString();
                ReassignSessionKeyTextBox.Text = ReassignSessionKey = Guid.NewGuid().ToString();
                RequestInformationSessionKeyTextBox.Text = RequestInformationSessionKey = Guid.NewGuid().ToString();
                FinishedSessionKeyTextBox.Text = FinishedSessionKey = Guid.NewGuid().ToString();
                SignatureVerificationSessionKeyTextBox.Text = SignatureVerificationSessionKey = Guid.NewGuid().ToString();
                DataQualityCompletedSessionKeyTextBox.Text = DataQualityCompletedSessionKey = Guid.NewGuid().ToString();
                initEventEditorSession();
            }
            else
            {
                ApprovedSessionKey = ApprovedSessionKeyTextBox.Text;
                RejectedSessionKey = RejectedSessionKeyTextBox.Text;
                TerminationSessionKey = TerminationSessionKeyTextBox.Text;
                ReassignSessionKey = ReassignSessionKeyTextBox.Text;
                RequestInformationSessionKey = RequestInformationSessionKeyTextBox.Text;
                FinishedSessionKey = FinishedSessionKeyTextBox.Text;
                SignatureVerificationSessionKey = SignatureVerificationSessionKeyTextBox.Text;
                DataQualityCompletedSessionKey = DataQualityCompletedSessionKeyTextBox.Text;
            }

            generateSession();
            switch (SPContext.Current.FormContext.FormMode)
            {
                case SPControlMode.New:
                    break;
                case SPControlMode.Edit:
                    //FormToolBar1.ControlMode = SPControlMode.New;
                    if (!IsPostBack)
                        ShowItem();
                    break;
                case SPControlMode.Display:
                    ShowItem();
                    LockUI();
                    FormReadOnly = true;
                    break;
            }

            if (!isValidApprovedRule()
                && string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID, true) != 0)
                ApprovedRuleErrorLabel.Visible = true;
            else
                ApprovedRuleErrorLabel.Visible = false;

            if (!isValidSignatureVerificationRule()
                && string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID, true) == 0)
                SignatureVerificationRuleErrorLabel.Visible = true;
            else
                SignatureVerificationRuleErrorLabel.Visible = false;

            if (!isValidDataQualityCompletedRule()
                && string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.DATA_QUALITY_CONTENT_TYPE_ID, true) == 0)
                DataQualityCompletedRuleErrorLabel.Visible = true;
            else
                DataQualityCompletedRuleErrorLabel.Visible = false;

        }

        private void initEventEditorSession()
        {
            string sessionID = Guid.NewGuid().ToString();
            _hiddenUniqueKey.Value = sessionID;


            SPListItem currentItem = SPContext.Current.ListItem;
            if (currentItem != null)
            {
                TaskEventSettings savedSetting = currentItem.GetCustomSettings<TaskEventSettings>(IOfficeFeatures.Infrastructure);
                if (savedSetting == null) savedSetting = new TaskEventSettings();
                ChangeLabelEvent(savedSetting);
                Session[sessionID] = savedSetting;
            }
        }

        private void ChangeLabelEvent(TaskEventSettings settings)
        {
            foreach (TaskEventSetting evnt in settings.Events)
            {
                Label status = this.FindControl("lb" + evnt.Type.ToString() + "Actions") as Label;
                status.CssClass = "EditedEvent";
                if (evnt.Actions.Count > 0)
                    status.Text = "<font color='green'>*</font>" + "<i> (" + evnt.Actions.Count.ToString() + ")</i>";
                else
                    status.Text = string.Empty;
            }
        }

        private void LockUI()
        {
            ConfigurationName.Enabled = false;
            txtTemplateUrl.Enabled = false;
            TaskContentTypesDropDown.Enabled = false;
            ApproversPeoplePicker.Enabled = false;
            DoNotExpandGroupCheckBox.Enabled = false;
            ExecutionModeRadioList.Enabled = false;
            AssigmentEmailTemplateDropDown.Enabled = false;
            DueDateDurationTextBox.Enabled = false;
            DueDateMessureDropDown.Enabled = false;
            ReminderDateDurationTextBox.Enabled = false;
            ReminderDateMessureDropDown.Enabled = false;
            ReminderEmailTemplateDropDown.Enabled = false;
            EscalationDateDurationTextBox.Enabled = false;
            EscalationDateMessureDropDown.Enabled = false;
            EscalationPartyPeoplePicker.Enabled = false;
            EscalationEmailTemplateDropDown.Enabled = false;
            RequireNumberCheckbox.Enabled = false;
            TasksTextBox.Enabled = false;
            SaveButtonTop.Visible = SaveButton.Visible = false;
            TaskContributorsPeoplePicker.Enabled = false;
            TaskObserversPeoplePicker.Enabled = false;
            createLink.Visible = false;
            TaskInstructionsTextBox.Enabled = false;
            TaskTitlePrefixTextBox.Enabled = false;
            AllowDueDateChangeRessignmentCheckBox.Enabled = false;
            AllowDueDateChangeRequestInfomationCheckBox.Enabled = false;
            RequestInfomationCheckBox.Enabled = false;
            ReassignCheckBox.Enabled = false;
            PlaceHoldOnCheckBox.Enabled = false;
            SendEECCheckBox.Enabled = false;
            ApproversDropDown.Enabled = false;
            UseMetadataAssignmentRadio.Enabled = false;
            SpecifiedParticipantsRadio.Enabled = false;
            ByPassTaskCheckBox.Enabled = false;
            IgnoreIfNoParticipantCheckBox.Enabled = false;
        }

        private void ShowItem()
        {
            SPListItem currentItem = SPContext.Current.ListItem;

            ConfigurationName.Text = (string)currentItem[TaskConfigurationFieldIds.CogfigName];
            txtTemplateUrl.Text = (string)currentItem[TaskConfigurationFieldIds.EmailTemplateUrl];
            PopulateEmailTemplateDropDowns();

            TaskContentTypesDropDown.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.TaskContentTypeId];

            showSettingsOfCCIappWorkflowTask(currentItem);

            if (currentItem[TaskConfigurationFieldIds.Approvers] != null)
            {
                SPFieldUserValueCollection users = (SPFieldUserValueCollection)currentItem[TaskConfigurationFieldIds.Approvers];
                foreach (SPFieldUserValue u in users)
                {
                    addPickerEntity(ApproversPeoplePicker, u);
                }
            }

            if (currentItem[TaskConfigurationFieldIds.TaskContributors] != null)
            {
                SPFieldUserValueCollection users = (SPFieldUserValueCollection)currentItem[TaskConfigurationFieldIds.TaskContributors];
                foreach (SPFieldUserValue u in users)
                {
                    addPickerEntity(TaskContributorsPeoplePicker, u);
                }
            }

            if (currentItem[TaskConfigurationFieldIds.TaskObservers] != null)
            {
                SPFieldUserValueCollection users = (SPFieldUserValueCollection)currentItem[TaskConfigurationFieldIds.TaskObservers];
                foreach (SPFieldUserValue u in users)
                {
                    addPickerEntity(TaskObserversPeoplePicker, u);
                }
            }

            if (currentItem[TaskConfigurationFieldIds.ExpandGroup] != null)
            {
                DoNotExpandGroupCheckBox.Checked = !((bool)currentItem[TaskConfigurationFieldIds.ExpandGroup]);
            }
            ExecutionModeRadioList.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.AssignmentType];
            AssigmentEmailTemplateDropDown.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.AssignmentEmailTemplate];

            if (currentItem[TaskConfigurationFieldIds.DueDateDuration] != null)
                DueDateDurationTextBox.Text = currentItem[TaskConfigurationFieldIds.DueDateDuration].ToString();

            DueDateMessureDropDown.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.DueDateMeasure];

            if (currentItem[TaskConfigurationFieldIds.ReminderDateDuration] != null)
                ReminderDateDurationTextBox.Text = currentItem[TaskConfigurationFieldIds.ReminderDateDuration].ToString();

            ReminderDateMessureDropDown.Text = (string)currentItem[TaskConfigurationFieldIds.ReminderDateMeasure];
            ReminderEmailTemplateDropDown.Text = (string)currentItem[TaskConfigurationFieldIds.ReminderEmailTemplate];

            if (currentItem[TaskConfigurationFieldIds.EscalationDateDuration] != null)
                EscalationDateDurationTextBox.Text = currentItem[TaskConfigurationFieldIds.EscalationDateDuration].ToString();

            EscalationDateMessureDropDown.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.EscalationDateMeasure];

            if (currentItem[TaskConfigurationFieldIds.EscalationParty] != null)
            {
                SPFieldUserValue u = new SPFieldUserValue(SPContext.Current.Web, (string)currentItem[TaskConfigurationFieldIds.EscalationParty]);
                addPickerEntity(EscalationPartyPeoplePicker, u);
            }

            if (currentItem[TaskConfigurationFieldIds.TaskInstructions] != null)
            {
                TaskInstructionsTextBox.Text = currentItem[TaskConfigurationFieldIds.TaskInstructions].ToString();
            }

            if (currentItem[TaskConfigurationFieldIds.TaskTitlePrefix] != null)
            {
                TaskTitlePrefixTextBox.Text = currentItem[TaskConfigurationFieldIds.TaskTitlePrefix].ToString();
            }

            EscalationEmailTemplateDropDown.SelectedValue = (string)currentItem[TaskConfigurationFieldIds.EscalationEmailTemplate];
            RequireNumberCheckbox.Checked = (bool)currentItem[TaskConfigurationFieldIds.UseNumberRequired];

            if (currentItem[TaskConfigurationFieldIds.NumberRequired] != null)
                TasksTextBox.Text = currentItem[TaskConfigurationFieldIds.NumberRequired].ToString();

            if (currentItem[TaskConfigurationFieldIds.UseMetaDataAssignment] != null)
            {
                SpecifiedParticipantsRadio.Checked = (bool)currentItem[TaskConfigurationFieldIds.UseMetaDataAssignment] == false;
                UseMetadataAssignmentRadio.Checked = (bool)currentItem[TaskConfigurationFieldIds.UseMetaDataAssignment] == true;
            }

            if (currentItem[TaskConfigurationFieldIds.ApproversFieldId] != null)
            {
                ApproversDropDown.SelectedValue = currentItem[TaskConfigurationFieldIds.ApproversFieldId].ToString();
            }

            if (currentItem[TaskConfigurationFieldIds.ByPassTask] != null)
            {
                ByPassTaskCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.ByPassTask];
                ByPassTaskDiv.Visible = ByPassTaskCheckBox.Checked;
            }

            if (currentItem.Fields.ContainFieldId(TaskConfigurationFieldIds.IgnoreIfNoParticipant) && currentItem[TaskConfigurationFieldIds.IgnoreIfNoParticipant] != null)
            {
                IgnoreIfNoParticipantCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.IgnoreIfNoParticipant];
            }

            showApproversSection();
            reloadSavedTaskEvent();
        }

        private void reloadSavedTaskEvent()
        {

        }

        private void showSettingsOfCCIappWorkflowTask(SPListItem currentItem)
        {
            if (isVisibleCCIappWorkflowTaskSettings())
            {
                WorkflowTaskSettingTable.Visible = true;
                ReassignCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.AllowReassign];
                PlaceHoldOnCheckBox.Checked = currentItem[TaskConfigurationFieldIds.AllowPlaceHoldOn] != null ? (bool)currentItem[TaskConfigurationFieldIds.AllowPlaceHoldOn] : true;
                SendEECCheckBox.Checked = currentItem[TaskConfigurationFieldIds.AllowSendEEC] != null ? (bool)currentItem[TaskConfigurationFieldIds.AllowSendEEC] : true;
                RequestInfomationCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.AllowRequestInfomation];
                AllowDueDateChangeRequestInfomationCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.AllowDueDateChangeRequestInfomation];
                AllowDueDateChangeRessignmentCheckBox.Checked = (bool)currentItem[TaskConfigurationFieldIds.AllowDueDateChangeRessignment];
                AllowDueDateChangeRequestInfomationCheckBox.Enabled = RequestInfomationCheckBox.Checked;
                AllowDueDateChangeRessignmentCheckBox.Enabled = ReassignCheckBox.Checked;
                CCIWorkflowTaskSettingDiv.Visible = true;
            }
            switchContentDisplay(currentItem[TaskConfigurationFieldIds.TaskContentTypeId].ToString().ToLower().Trim());
            //switch (currentItem[ApprovalConfigFieldId.TaskContentTypeId].ToString().ToLower().Trim())
            //{
            //    case Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID:
                
            //        {
            //            SignatureVerificationRuleErrorLabel.Visible = true;
            //            SignatureVerificationTaskRuleDiv.Visible = true;
            //            AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = false;
            //            DataQualityCompletedTaskRuleDiv.Visible = false;
            //            break;
            //        }
            //    case Constants.DATA_QUALITY_CONTENT_TYPE_ID:
            //        {
            //            DataQualityCompletedTaskRuleDiv.Visible = true;
            //            DataQualityCompletedRuleErrorLabel.Visible = true;
            //            SignatureVerificationTaskRuleDiv.Visible = false;
            //            AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = false;
            //            break;
            //        }
            //    default:
            //        {
            //            SignatureVerificationTaskRuleDiv.Visible = false;
            //            SignatureVerificationRuleErrorLabel.Visible = false;
            //            DataQualityCompletedTaskRuleDiv.Visible = false;
            //            AllTaskRuleOfCCIappWorkflowTaskDiv.Visible = true;
            //            ApprovedRuleErrorLabel.Visible = true;
            //            WorkflowTaskSettingTable.Visible = CCIWorkflowTaskSettingDiv.Visible = isVisibleCCIappWorkflowTaskSettings();
            //            break;
            //        }
            //}
        }

        private bool isVisibleCCIappWorkflowTaskSettings()
        {
            SPContentTypeId cciappWorkflowTaskCT = new SPContentTypeId(Constants.CCI_WORKFLOW_TASK_ID);
            SPContentTypeId selectedCT = new SPContentTypeId(TaskContentTypesDropDown.SelectedValue);
            if (selectedCT.IsChildOf(cciappWorkflowTaskCT))
                return true;
            return false;
        }

        private void PreloadData()
        {
            List<SPContentType> contentTypes = SPContext.Current.Web.FindAllContentTypesOf(SPBuiltInContentTypeId.WorkflowTask);
            TaskContentTypesDropDown.DataSource = contentTypes;
            TaskContentTypesDropDown.DataTextField = "Name";
            TaskContentTypesDropDown.DataValueField = "Id";
            TaskContentTypesDropDown.DataBind();

            SPContentTypeId selectedCTId = new SPContentTypeId(TaskContentTypesDropDown.SelectedValue);

            SPContentType selectedCT = contentTypes.FirstOrDefault(ct => ct.Id == selectedCTId);

            DueDateMessureDropDown.DataSource = new string[] { "Days", "Weeks" };
            DueDateMessureDropDown.DataBind();

            ReminderDateMessureDropDown.DataSource = new string[] { "Days", "Weeks" };
            ReminderDateMessureDropDown.DataBind();

            EscalationDateMessureDropDown.DataSource = new string[] { "Days", "Weeks" };
            EscalationDateMessureDropDown.DataBind();

            List<SPField> fieldsApprovers = SPContext.Current.Web.FindAllFieldOfType(SPFieldType.User);
            ApproversDropDown.DataSource = fieldsApprovers;
            ApproversDropDown.DataTextField = "Title";
            ApproversDropDown.DataValueField = "Id";
            ApproversDropDown.DataBind();

        }       

        private void PopulateEmailTemplateDropDowns()
        {
            string listMailTemplateUrl = txtTemplateUrl.Text;
            if (string.IsNullOrEmpty(listMailTemplateUrl))
            {
                return;
            }
            AssigmentEmailTemplateDropDown.Items.Clear();
            ReminderEmailTemplateDropDown.Items.Clear();
            EscalationEmailTemplateDropDown.Items.Clear();

            DataTable items = GetEmailTemplates(listMailTemplateUrl);
            if (items != null)
            {
                DataView dvOptions = new DataView(items);
                dvOptions.Sort = "Title";

                AssigmentEmailTemplateDropDown.DataSource = dvOptions;
                AssigmentEmailTemplateDropDown.DataTextField = "Title";
                AssigmentEmailTemplateDropDown.DataValueField = "Title";
                AssigmentEmailTemplateDropDown.DataBind();
                AssigmentEmailTemplateDropDown.Items.Insert(0, new ListItem("Select Email Template", string.Empty));

                ReminderEmailTemplateDropDown.DataSource = dvOptions;
                ReminderEmailTemplateDropDown.DataTextField = "Title";
                ReminderEmailTemplateDropDown.DataValueField = "Title";
                ReminderEmailTemplateDropDown.DataBind();
                ReminderEmailTemplateDropDown.Items.Insert(0, new ListItem("Select Email Template", string.Empty));

                EscalationEmailTemplateDropDown.DataSource = dvOptions;
                EscalationEmailTemplateDropDown.DataTextField = "Title";
                EscalationEmailTemplateDropDown.DataValueField = "Title";
                EscalationEmailTemplateDropDown.DataBind();
                EscalationEmailTemplateDropDown.Items.Insert(0, new ListItem("Select Email Template", string.Empty));
            }
        }

        private DataTable GetEmailTemplates(string emailTemplateListUrl)
        {
            DataTable items = null;
            try
            {
                SPList list = CCIUtility.GetListFromURL(emailTemplateListUrl);
                items = list.Items.GetDataTable();
                UrlErrorMessageLabel.Visible = false;
            }
            catch
            {
                if (!string.IsNullOrEmpty(emailTemplateListUrl))
                    UrlErrorMessageLabel.Visible = true;
            }
            return items;
        }

        public void SaveButton_Click(object sender, EventArgs e)
        {
            if (!IsFormValid()) return;
            if (string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID, true) != 0
                && string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.DATA_QUALITY_CONTENT_TYPE_ID, true) != 0
                && !isValidApprovedRule())
                return;

            if (string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID, true) == 0
                && !isValidSignatureVerificationRule())
                return;

            if (string.Compare(TaskContentTypesDropDown.SelectedValue, Constants.DATA_QUALITY_CONTENT_TYPE_ID, true) == 0
                && !isValidDataQualityCompletedRule())
                return;

            switch (SPContext.Current.FormContext.FormMode)
            {
                case SPControlMode.New:
                    SPListItem newItem = SPContext.Current.List.Items.Add();
                    UpdateItem(newItem);
                    break;

                case SPControlMode.Edit:
                    UpdateItem(SPContext.Current.ListItem);
                    break;
            }
            //if (!string.IsNullOrEmpty(Request.QueryString["Source"]))
            //{
            //    Response.Redirect(Request.QueryString["Source"]);
            //}
            back();
        }

        private void back()
        {
            if (IsDialog)
                closePopup();
            else
                if (string.IsNullOrEmpty(SourceUrl))
                    SPUtility.Redirect(SPContext.Current.Web.Url, SPRedirectFlags.Default, this.Context);
                else
                    SPUtility.Redirect(SPEncode.UrlDecodeAsUrl(SourceUrl), SPRedirectFlags.Default, this.Context);
        } 

        private void closePopup() {
            //Close popup
            Context.Response.Clear();
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();

        }

        private bool IsFormValid()
        {
            GetPeople(EscalationPartyPeoplePicker);
            GetPeople(TaskContributorsPeoplePicker);
            GetPeople(TaskObserversPeoplePicker);
            GetPeople(ApproversPeoplePicker);
            if (SpecifiedParticipantsRadio.Checked == true && ApproversPeoplePicker.Entities.Count == 0)
            {                
                ApproversPeoplePicker.IsValid = false;
                ApproversPeoplePicker.ErrorMessage = "You must specify a value for this required field";
            }
            if (UseMetadataAssignmentRadio.Checked)
            {
                validApproversPeoplePicker.IsValid = true;                
            }
            return (ApproversPeoplePicker.IsValid
                  && validApproversPeoplePicker.IsValid
                  && validEscalationPartyPeoplePicker.IsValid
                  && validTaskContributorsPeoplePicker.IsValid
                  && validTaskObserversPeoplePicker.IsValid);
        }

        private void UpdateItem(SPListItem listItem)
        {
            listItem[TaskConfigurationFieldIds.CogfigName] = ConfigurationName.Text;
            listItem[TaskConfigurationFieldIds.TaskContentTypeId] = TaskContentTypesDropDown.SelectedValue;
            listItem[TaskConfigurationFieldIds.UseMetaDataAssignment] = UseMetadataAssignmentRadio.Checked;

            if (!UseMetadataAssignmentRadio.Checked)
            {
                listItem[TaskConfigurationFieldIds.Approvers] = GetPeople(ApproversPeoplePicker);
                listItem[TaskConfigurationFieldIds.ApproversFieldId] = null;
            }
            else
            {
                listItem[TaskConfigurationFieldIds.ApproversFieldId] = ApproversDropDown.SelectedItem.Value;
                listItem[TaskConfigurationFieldIds.Approvers] = null;
            }

            listItem[TaskConfigurationFieldIds.ExpandGroup] = !DoNotExpandGroupCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AssignmentType] = ExecutionModeRadioList.SelectedValue;
            listItem[TaskConfigurationFieldIds.AssignmentEmailTemplate] = AssigmentEmailTemplateDropDown.SelectedValue;
            listItem[TaskConfigurationFieldIds.DueDateDuration] = DueDateDurationTextBox.Text;
            listItem[TaskConfigurationFieldIds.DueDateMeasure] = DueDateMessureDropDown.SelectedValue;
            listItem[TaskConfigurationFieldIds.ReminderDateDuration] = ReminderDateDurationTextBox.Text;
            listItem[TaskConfigurationFieldIds.ReminderDateMeasure] = ReminderDateMessureDropDown.Text;
            listItem[TaskConfigurationFieldIds.ReminderEmailTemplate] = ReminderEmailTemplateDropDown.Text;
            listItem[TaskConfigurationFieldIds.EscalationDateDuration] = EscalationDateDurationTextBox.Text;
            listItem[TaskConfigurationFieldIds.EscalationDateMeasure] = EscalationDateMessureDropDown.SelectedValue;
            listItem[TaskConfigurationFieldIds.EscalationParty] = GetPeople(EscalationPartyPeoplePicker);
            listItem[TaskConfigurationFieldIds.EscalationEmailTemplate] = EscalationEmailTemplateDropDown.SelectedValue;
            listItem[TaskConfigurationFieldIds.UseNumberRequired] = RequireNumberCheckbox.Checked;
            listItem[TaskConfigurationFieldIds.NumberRequired] = TasksTextBox.Text;
            listItem[TaskConfigurationFieldIds.EmailTemplateUrl] = txtTemplateUrl.Text;
            listItem[TaskConfigurationFieldIds.TaskContributors] = GetPeople(TaskContributorsPeoplePicker);
            listItem[TaskConfigurationFieldIds.TaskObservers] = GetPeople(TaskObserversPeoplePicker);

            listItem[TaskConfigurationFieldIds.TaskInstructions] = TaskInstructionsTextBox.Text;
            listItem[TaskConfigurationFieldIds.TaskTitlePrefix] = TaskTitlePrefixTextBox.Text;
            listItem[TaskConfigurationFieldIds.AllowReassign] = ReassignCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AllowRequestInfomation] = RequestInfomationCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AllowDueDateChangeRessignment] = AllowDueDateChangeRessignmentCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AllowDueDateChangeRequestInfomation] = AllowDueDateChangeRequestInfomationCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AllowPlaceHoldOn] = PlaceHoldOnCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.AllowSendEEC] = SendEECCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.ByPassTask] = ByPassTaskCheckBox.Checked;
            listItem[TaskConfigurationFieldIds.IgnoreIfNoParticipant] = IgnoreIfNoParticipantCheckBox.Checked;

            updateStatusRule(listItem);
            updateTaskEvent(listItem);
            listItem.Update();
        }

        private void updateTaskEvent(SPListItem listItem)
        {
            TaskEventSettings settings = Session[_hiddenUniqueKey.Value] as TaskEventSettings;
            if (settings == null) settings = new TaskEventSettings();
            listItem.SetCustomSettings<TaskEventSettings>(IOfficeFeatures.Infrastructure, settings);
            //clear edit event
            Session.Remove(_hiddenUniqueKey.Value);
            _hiddenUniqueKey.Value = string.Empty;
        }

        private bool isValidApprovedRule()
        {
            if (Session[ApprovedSessionKey] == null) return false;
            List<Criteria> approvedRule = (List<Criteria>)Session[ApprovedSessionKey];
            if (approvedRule == null) return false;
            return approvedRule.Count > 0 ? true : false;
        }

        private bool isValidDataQualityCompletedRule()
        {
            if (Session[DataQualityCompletedSessionKey] == null) return false;
            List<Criteria> dataQualityCompletedSessionKey = (List<Criteria>)Session[DataQualityCompletedSessionKey];
            if (dataQualityCompletedSessionKey == null) return false;
            return dataQualityCompletedSessionKey.Count > 0 ? true : false;
        }

        private bool isValidSignatureVerificationRule()
        {
            if (Session[SignatureVerificationSessionKey] == null) return false;
            List<Criteria> signatureVerificationRule = (List<Criteria>)Session[SignatureVerificationSessionKey];
            if (signatureVerificationRule == null) return false;
            return signatureVerificationRule.Count > 0 ? true : false;
        }

        private void generateSession()
        {
            if (Session[ApprovedSessionKey] != null
                || Session[RejectedSessionKey] != null
                || Session[TerminationSessionKey] != null
                || Session[ReassignSessionKey] != null
                || Session[RequestInformationSessionKey] != null
                || Session[FinishedSessionKey] != null
                || Session[SignatureVerificationSessionKey] != null
                || Session[DataQualityCompletedSessionKey] != null) return;

            Session[ApprovedSessionKey] = new List<Criteria>();
            Session[RejectedSessionKey] = new List<Criteria>();
            Session[TerminationSessionKey] = new List<Criteria>();
            Session[ReassignSessionKey] = new List<Criteria>();
            Session[RequestInformationSessionKey] = new List<Criteria>();
            Session[FinishedSessionKey] = new List<Criteria>();
            Session[SignatureVerificationSessionKey] = new List<Criteria>();
            Session[DataQualityCompletedSessionKey] = new List<Criteria>();
            if (SPContext.Current.ListItem != null)
            {
                TaskRuleSettings settings = SPContext.Current.ListItem.GetCustomSettings<TaskRuleSettings>(IOfficeFeatures.Infrastructure);
                if (settings == null) return;
                Session[ApprovedSessionKey] = settings.ApprovedCriteriaList;
                Session[RejectedSessionKey] = settings.RejectedCriteriaList;
                Session[TerminationSessionKey] = settings.TerminationCriteriaList;
                Session[ReassignSessionKey] = settings.ReassignCriteriaList;
                Session[RequestInformationSessionKey] = settings.RequestInformationCriteriaList;
                Session[FinishedSessionKey] = settings.FinishedCriteriaList;
                Session[SignatureVerificationSessionKey] = settings.SignatureVerificationCriteriaList;
                Session[DataQualityCompletedSessionKey] = settings.DataQualityCompletedCriteriaList;
            }
        }

        private void updateStatusRule(SPListItem item)
        {
            TaskRuleSettings settings = item.GetCustomSettings<TaskRuleSettings>(IOfficeFeatures.Infrastructure);
            if (settings == null)
                settings = new TaskRuleSettings();
            if (Session[ApprovedSessionKey] != null)
            {
                settings.ApprovedCriteriaList = (List<Criteria>)Session[ApprovedSessionKey];
                Session.Remove(ApprovedSessionKey);
            }

            if (Session[RejectedSessionKey] != null)
            {
                settings.RejectedCriteriaList = (List<Criteria>)Session[RejectedSessionKey];
                Session.Remove(RejectedSessionKey);
            }

            if (Session[TerminationSessionKey] != null)
            {
                settings.TerminationCriteriaList = (List<Criteria>)Session[TerminationSessionKey];
                Session.Remove(TerminationSessionKey);
            }

            if (Session[ReassignSessionKey] != null)
            {
                settings.ReassignCriteriaList = (List<Criteria>)Session[ReassignSessionKey];
                Session.Remove(ReassignSessionKey);
            }

            if (Session[RequestInformationSessionKey] != null)
            {
                settings.RequestInformationCriteriaList = (List<Criteria>)Session[RequestInformationSessionKey];
                Session.Remove(RequestInformationSessionKey);
            }

            if (Session[FinishedSessionKey] != null)
            {
                settings.FinishedCriteriaList = (List<Criteria>)Session[FinishedSessionKey];
                Session.Remove(FinishedSessionKey);
            }

            if (Session[SignatureVerificationSessionKey] != null)
            {
                settings.SignatureVerificationCriteriaList = (List<Criteria>)Session[SignatureVerificationSessionKey];
                Session.Remove(SignatureVerificationSessionKey);
            }

            if (Session[DataQualityCompletedSessionKey] != null)
            {
                settings.DataQualityCompletedCriteriaList = (List<Criteria>)Session[DataQualityCompletedSessionKey];
                Session.Remove(DataQualityCompletedSessionKey);
            }
            item.SetCustomSettings<TaskRuleSettings>(IOfficeFeatures.Infrastructure, settings);
        }

        private SPFieldUserValueCollection GetPeople(PeopleEditor peoplePicker)
        {
            SPFieldUserValueCollection users = new SPFieldUserValueCollection();

            foreach (PickerEntity entity in peoplePicker.ResolvedEntities)
            {
                SPFieldUserValue user;
                if (entity.EntityData[PeopleEditorEntityDataKeys.PrincipalType].ToString() == "User")
                {
                    user = new SPFieldUserValue(SPContext.Current.Web,
                                    Convert.ToInt32(entity.EntityData[PeopleEditorEntityDataKeys.UserId]),
                                    entity.Description);
                    if (user.User == null)
                    {
                        //peoplePicker.IsValid = false;
                        //peoplePicker.ErrorMessage = "The participants is not a member of site";
                        CustomValidator control = this.FindControl("valid" + peoplePicker.ID) as CustomValidator;
                        if (control != null) control.IsValid = false;
                    }
                }
                else
                {
                    user = new SPFieldUserValue(SPContext.Current.Web,
                                Convert.ToInt32(entity.EntityData[PeopleEditorEntityDataKeys.SharePointGroupId]),
                                entity.Description);
                }
                users.Add(user);
            }
            return (users.Count > 0) ? users : null;
        }

        private void addPickerEntity(PeopleEditor peoplePicker, SPFieldUserValue userValue)
        {
            if (userValue != null && userValue.LookupValue != null)
            {
                PickerEntity pe = new PickerEntity();
                pe.Key = userValue.LookupValue.ToString();
                pe = peoplePicker.ValidateEntity(pe);
                peoplePicker.Entities.Add(pe);
            }
        }
    }
}
