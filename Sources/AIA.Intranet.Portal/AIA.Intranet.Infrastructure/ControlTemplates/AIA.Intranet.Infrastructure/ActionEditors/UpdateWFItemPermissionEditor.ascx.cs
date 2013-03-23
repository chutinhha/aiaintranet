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
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class UpdateWFItemPermissionEditor : ActionEditorControl
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
            //ddlTaskColumn.SelectedIndexChanged += new EventHandler(ddlTaskColumn_SelectedIndexChanged);
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
            //var field = fields.Where(p => p.Id.ToString() == ddlTaskColumn.SelectedValue).FirstOrDefault();
            //if (field != null)
            //{
            //    this.EditingFieldId = field.Id;
            //}

            //control = base.BuildValueSelectorControl(field, value);
            //ltrHolder.Controls.Clear();
            //ltrHolder.Controls.Add(control);
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
                PreloadData();
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
            //ltrHolder.Controls.Add(control);
        }
        
        private void PreloadData()
        {
            var defs = SPContext.Current.Web.RoleDefinitions;

            ddlRoleDefinitions.DataSource = defs;
            
            ddlRoleDefinitions.DataTextField = "Name";
            ddlRoleDefinitions.DataValueField = "Id";
            ddlRoleDefinitions.DataBind();

            var sources = SPContext.Current.List.Fields.Cast<SPField>().Where(p=>!p.Hidden && p.Type == SPFieldType.User).OrderBy(p=>p.Title);
            foreach (var item in sources)
	        {
                MultiLookupPicker.AddItem(item.Id.ToString(), item.Title, item.Description, "");
	        }
            
        }
        /// <summary>
        
        /// </summary>
        /// <returns></returns>
        private List<SPField> GetAvaiableFields()
        {
            return new List<SPField>();
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

            var staticUsers = new List<string>();

            foreach (PickerEntity entity in peSpecificUsersGroups.ResolvedEntities)
            {
                staticUsers.Add(entity.Key);
            }

            return new UpdateWFItemPermissionSettings()
            {
                RoleId = ddlRoleDefinitions.SelectedValue,
                AllParticipiants = chkApprovers.Checked,
                KeepExisting=chkKeepExisting.Checked,
                Columns = MultiLookupPicker.SelectedIds.Cast<string>().ToList(),
                StaticUsers = staticUsers
            };

            
        }
        
        private void displayActionData()
        {
            PreloadData();

            if (Action == null) return;
            UpdateWFItemPermissionSettings savedAction = (UpdateWFItemPermissionSettings)Action;
            ddlRoleDefinitions.SelectedValue = savedAction.RoleId;
            foreach (string item in savedAction.Columns)
            {
                var field = SPContext.Current.List.Fields[new Guid(item)];

                MultiLookupPicker.AddSelectedItem(item, field.Title);
            }
            chkKeepExisting.Checked = savedAction.KeepExisting;
            chkApprovers.Checked = savedAction.AllParticipiants;
            
            peSpecificUsersGroups.CommaSeparatedAccounts = string.Join(",", savedAction.StaticUsers.ToArray());
        }
    }
}
