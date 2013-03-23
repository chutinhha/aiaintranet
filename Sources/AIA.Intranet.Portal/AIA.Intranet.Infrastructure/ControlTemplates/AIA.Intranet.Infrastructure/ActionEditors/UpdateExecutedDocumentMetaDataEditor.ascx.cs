using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AIA.Intranet.Common;
using AIA.Intranet.Common.Helpers;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using System.Reflection;
using Microsoft.SharePoint.WebPartPages;
using System.Text;
using AIA.Intranet.Model.Workflow;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class UpdateExecutedDocumentMetaDataEditor : ActionEditorControl
    {        
        protected DropDownList ddlItemColumn;
        protected CheckBox chkRemoveAction;
        protected Literal lbScript;
        protected PlaceHolder ltrHolder;
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
        protected override void OnPreRender(EventArgs e)
        {
            if (chkRemoveAction.Checked)
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type=\"text/javascript\" language=\"javascript\">");
                script.AppendFormat("       _spBodyOnLoadFunctionNames.push(\"{0}_click(document.getElementById('{1}'))\");", chkRemoveAction.ClientID, chkRemoveAction.ClientID);
                script.Append("</script>");
                lbScript.Text = script.ToString();
            }
            base.OnPreRender(e);
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
            List<SPField> fields = getAvailableColumns();
            var field = fields.Where(p => p.Id.ToString() == ddlItemColumn.SelectedValue).FirstOrDefault();
            if (field != null)
            {
                this.EditingFieldId = field.Id;
            }

            control = base.BuildValueSelectorControl(field, value);
            ltrHolder.Controls.Clear();
            ltrHolder.Controls.Add(control);
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
            return fields.Where(p => !p.Hidden && p.Title != "Predecessors" && p.Title != "Related Issues")
                .OrderBy(p => p.Title)
                .ToList();;
            
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
            return new UpdateExecutedDocumentMetaDataEditorSettings()
            {
                FieldId = ddlItemColumn.SelectedValue,
                Value = base.GetControlValue(control, field)
            };
        }
        private void displayActionData()
        {
            showSiteColumns();            
            if (Action == null) return;
            UpdateExecutedDocumentMetaDataEditorSettings savedAction = (UpdateExecutedDocumentMetaDataEditorSettings)Action;            
            ddlItemColumn.SelectedValue = savedAction.FieldId;
            ShowCurrentSelector(savedAction.Value);            
       }
    }
}
