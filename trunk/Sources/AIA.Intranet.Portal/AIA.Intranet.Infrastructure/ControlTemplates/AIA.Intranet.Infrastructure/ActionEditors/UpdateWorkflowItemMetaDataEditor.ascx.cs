using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Workflow;
using AIA.Intranet.Common.Utilities;
using System.Text;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class UpdateWorkflowItemMetaDataEditor : ActionEditorControl
    {
        private Control control;
        public Guid EditingFieldId
        {
            get
            {
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
            ddlItemColumn.SelectedIndexChanged += new EventHandler(ddlItemColumn_SelectedIndexChanged);
            chkRemoveAction.Attributes.Add("onclick", string.Format("{0}_click(this);", chkRemoveAction.ClientID));
            base.OnInit(e);
        }

        void ddlItemColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCurrentSelector(string.Empty);
        }

        private void ShowCurrentSelector(string value)
        {
            try
            {
                List<SPField> fields = getAvailableColumns();
                if (fields == null) CCIUtility.LogError("fields = null", "DEBUG");
                if (ddlItemColumn == null) CCIUtility.LogError("ddlItemColumn = null", "DEBUG");
                var field = fields.Where(p => p.Id.ToString() == ddlItemColumn.SelectedValue).FirstOrDefault();
                if (field != null)
                {
                    this.EditingFieldId = field.Id;

                };

                if (ltrHolder == null) CCIUtility.LogError("ltrHolder = null", "DEBUG");
                control = base.BuildValueSelectorControl(field, value);
                ltrHolder.Controls.Clear();
                ltrHolder.Controls.Add(control);
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message + ex.StackTrace, Model.IOfficeFeatures.Infrastructure);
            }
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
                showSiteColumns();
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

        private void ReloadEditorControl()
        {
            List<SPField> fields = getAvailableColumns();
            SPField field = null;

            if (this.EditingFieldId != Guid.Empty)
            {
                Guid id = this.EditingFieldId;
                field = fields.Where(p => p.Id == id).FirstOrDefault();
            }

            control = base.BuildValueSelectorControl(field, "");
            ltrHolder.Controls.Add(control);
        }
        
        protected List<SPField> getAvailableColumns()
        {
            IEnumerable<SPField> fields = null;

            if (!string.IsNullOrEmpty(base.ContentTypeId))
            {
                //Load user colum from content type.
                SPContentType ct = base.GetContentType();
                if (ct != null)
                {
                    fields = ct.Fields.Cast<SPField>();
                }
            }
            else
            {
                fields = SPContext.Current.Web.AvailableFields.Cast<SPField>();
            }
            return fields.Where(p => !p.Hidden && p.Title != "Predecessors" && p.Title != "Related Issues" && p.Hidden == false)
                .OrderBy(p => p.Title)
                .ToList(); ;

        }
        private void showSiteColumns()
        {
            List<SPField> userFields = getAvailableColumns();
            ControlHelper.LoadFieldsToDropdown(ddlItemColumn, userFields);
        }

        public override TaskActionSettings GetAction()
        {
            EnsureChildControls();
            if (chkRemoveAction.Checked) return null;
            SPField field = getAvailableColumns().Where(p => p.Id.ToString() == ddlItemColumn.SelectedValue).First();

            return new UpdateWorkflowItemMetadataSettings()
            {
                FieldId = ddlItemColumn.SelectedValue,                
                Value = base.GetControlValue(control, field)
            };
        }

        private void displayActionData()
        {
            showSiteColumns();

            if (Action == null) return;

            UpdateWorkflowItemMetadataSettings savedAction = (UpdateWorkflowItemMetadataSettings)Action;            
            ddlItemColumn.SelectedValue = savedAction.FieldId;

            ShowCurrentSelector(savedAction.Value);

        }

    }
}
