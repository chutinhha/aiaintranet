using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Workflow;
using System.Collections.Generic;
using System.Text;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class UpdateWorkflowTaskMetaDataEditor : ActionEditorControl
    {
        private Control control;
        public Guid EditingFieldId
        {
            get {
                if (ViewState["EditingFieldId"] == null) return Guid.Empty;
                return (Guid)ViewState["EditingFieldId"];
            }
            set
            {
                ViewState["EditingFieldId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ddlTaskColumn.SelectedIndexChanged += new EventHandler(ddlTaskColumn_SelectedIndexChanged);
            chkRemoveAction.Attributes.Add("onclick", string.Format("{0}_click(this);", chkRemoveAction.ClientID));

            base.OnInit(e);
        }
        

        void ddlTaskColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCurrentSelector(string.Empty);
        }

        private void ShowCurrentSelector( string value)
        {
            List<SPField> fields = GetAvaiableFields();
            var field = fields.Where(p => p.Id.ToString() == ddlTaskColumn.SelectedValue).FirstOrDefault();
            if (field != null)
            {
                this.EditingFieldId = field.Id;
            }

            control = base.BuildValueSelectorControl(field, value);
            ltrHolder.Controls.Clear();
            ltrHolder.Controls.Add(control);
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (chkRemoveAction.Checked)
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type=\"text/javascript\" language=\"javascript\">");
                script.AppendFormat("       _spBodyOnLoadFunctionNames.push(\"{0}_click(document.getElementById('{1}'))\");", chkRemoveAction.ClientID, chkRemoveAction.ClientID);
                script.Append("</script>");
                ltrScript.Text = script.ToString();
            }
            base.OnPreRender(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (IsFirstLoad)
            {
                ShowAvailableUpdateFields();
            }
            if (!IsPostBack)
            {
                displayActionData();
            }
            else
            {
                ReloadEditorControl();
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// This method to reload current editor controls from postback, initial from old setting....
        /// </summary>
        private void ReloadEditorControl()
        {
            List<SPField> fields = GetAvaiableFields();
            SPField field = null;

            if (this.EditingFieldId != Guid.Empty)
            {
                Guid id = this.EditingFieldId;
                field = fields.Where(p => p.Id == id).FirstOrDefault();
            }

            control = base.BuildValueSelectorControl(field, "");
            ltrHolder.Controls.Add(control);
        }
        
        private void ShowAvailableUpdateFields()
        {
            List<SPField> fields = GetAvaiableFields();
            ControlHelper.LoadFieldsToDropdown(ddlTaskColumn, fields);                   
        }
        /// <summary>
        
        /// </summary>
        /// <returns></returns>
        private List<SPField> GetAvaiableFields()
        {
            string ctid = Request["taskContentTypeId"];
            SPContentType editingContentType = SPContext.Current.Web.AvailableContentTypes
                .Cast<SPContentType>()
                .Where(p => p.Id.ToString() == ctid)
                .FirstOrDefault();

            List<SPField> fields = editingContentType.Fields
                .Cast<SPField>()
                .Where(p => !p.Hidden && p.Title != "Predecessors"
                    && p.Title != "Related Issues"
                    && p.Id != SPBuiltInFieldId.AssignedTo
                    && p.Id != SPBuiltInFieldId.Priority
                    && p.Id != SPBuiltInFieldId.TaskDueDate
                    && p.Id != SPBuiltInFieldId.TaskStatus
                    && p.Id != SPBuiltInFieldId.Body
                    && p.Id != SPBuiltInFieldId.Completed
                    && p.Id != SPBuiltInFieldId.Outcome)

                .OrderBy(p => p.Title)
                .ToList();
            //remove all builtin task form.

            return fields;
        }
        public override TaskActionSettings GetAction()
        {
            EnsureChildControls();
            if (chkRemoveAction.Checked) return null;
            SPField field = GetAvaiableFields().Where(p=>p.Id.ToString() == ddlTaskColumn.SelectedValue).First();

            return new UpdateWorkflowTaskMetadataSettings()
            {
                FieldId = ddlTaskColumn.SelectedValue,
                Value =base.GetControlValue(control, field)
            };
        }
        
        private void displayActionData()
        {
            ShowAvailableUpdateFields();

            if (Action == null) return;
            UpdateWorkflowTaskMetadataSettings savedAction = (UpdateWorkflowTaskMetadataSettings)Action;
            txtItemValue.Text = savedAction.Value;
            ddlTaskColumn.SelectedValue = savedAction.FieldId;

            ShowCurrentSelector(savedAction.Value);
        }
    }
}
