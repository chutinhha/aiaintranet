using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Model.Workflow;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Text;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class SendEmailtoWorkflowTaskUserColumnEditor : ActionEditorControl
    {
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
        protected override void OnInit(EventArgs e)
        {
            chkRemoveAction.Attributes.Add("onclick", string.Format("{0}_click(this);", chkRemoveAction.ClientID));
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (IsFirstLoad)
            {
                _showTaskUserColumn();
            }
            if (!IsPostBack)
            {
                _displayActionData();
            }

            base.OnLoad(e);
        }

        private void _displayActionData()
        {
            _showTaskUserColumn();

            if (Action == null) return;
            SendEmailtoWorkflowTaskUserColumnSettings savedAction = (SendEmailtoWorkflowTaskUserColumnSettings)Action;
            TaskEmailTemplateSelector.TemplateName = savedAction.EmailTemplateName;
            TaskEmailTemplateSelector.TemplateUrl = savedAction.EmailTemplateUrl;
            ddlPersonOrGroupColumn.SelectedValue = savedAction.FieldId;
            ckbAttachTaskLink.Checked = savedAction.AttachTaskLink;
        }


        private void _showTaskUserColumn()
        {
            string ctid = Request["taskContentTypeId"];

            SPContentType editingContentType = SPContext.Current.Web.AvailableContentTypes
                .Cast<SPContentType>()
                .Where(p => p.Id.ToString() == ctid)
                .FirstOrDefault();

            List<SPField> userFields = editingContentType.Fields
                .Cast<SPField>()
                .Where(p => p.Type == SPFieldType.User)
                .OrderBy(p => p.Title)
                .ToList();

            ddlPersonOrGroupColumn.Items.Clear();
            ddlPersonOrGroupColumn.Items.Add(new ListItem() { Text = "Select a column", Value = string.Empty });
            foreach (SPField field in userFields)
            {
                ddlPersonOrGroupColumn.Items.Add(new ListItem() { Text = field.Title, Value = field.Id.ToString() });
            }
        }
        public override TaskActionSettings GetAction()
        {
            EnsureChildControls();
            if (chkRemoveAction.Checked) return null;
            return new SendEmailtoWorkflowTaskUserColumnSettings()
            {
                EmailTemplateName = TaskEmailTemplateSelector.TemplateName,
                EmailTemplateUrl = TaskEmailTemplateSelector.TemplateUrl,
                FieldId = ddlPersonOrGroupColumn.SelectedValue,
                AttachTaskLink = ckbAttachTaskLink.Checked
            };
        }
    }
}
