using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq;
using AIA.Intranet.Model.Workflow;
using System.Text;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class SendEmailToWfItemUserColumnEditor : ActionEditorControl
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
                showUserSiteColumn();
            }
            if (!IsPostBack)
            {
                displayActionData();
            }

            base.OnLoad(e);
        }

        private void displayActionData()
        {
            showUserSiteColumn();

            if (Action == null) return;
            SendEmailToWfItemUserColumnSettings savedAction = (SendEmailToWfItemUserColumnSettings)Action;
            TaskEmailTemplateSelector.TemplateName = savedAction.EmailTemplateName;
            TaskEmailTemplateSelector.TemplateUrl = savedAction.EmailTemplateUrl;
            ckbAttachTaskLink.Checked = savedAction.AttachTaskLink;
            ddlPersonOrGroupColumn.SelectedValue = savedAction.FieldId;
        }


        protected List<SPField> getAvailableUserColumns()
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
            List<SPField> results = fields.Where(p => !p.Hidden && (p.Type == SPFieldType.User || p.TypeAsString == "LookupFieldWithPicker"))
                .OrderBy(p => p.Title)
                .ToList(); ;

            results.Add(SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Created_x0020_By]);
            results.Add(SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Modified_x0020_By]);

            return results;

        }

        private void showUserSiteColumn()
        {
            List<SPField> userFields = getAvailableUserColumns();
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
            return new SendEmailToWfItemUserColumnSettings()
            {
                EmailTemplateName = TaskEmailTemplateSelector.TemplateName,
                EmailTemplateUrl = TaskEmailTemplateSelector.TemplateUrl,
                FieldId = ddlPersonOrGroupColumn.SelectedValue,
                AttachTaskLink = ckbAttachTaskLink.Checked
            };
        }
    }
}
