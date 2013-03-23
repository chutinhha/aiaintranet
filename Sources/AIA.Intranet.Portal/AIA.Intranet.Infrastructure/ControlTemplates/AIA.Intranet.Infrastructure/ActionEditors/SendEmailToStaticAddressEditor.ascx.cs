using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Model.Workflow;
using System.Text;
using AIA.Intranet.Common.Services;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class SendEmailToStaticAddressEditor : ActionEditorControl
    {
        public EmailTemplateSelector EmailSelector { get {
            return TaskEmailTemplateSelector as EmailTemplateSelector;
        } }
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
                //var maillist = ConfiguratioinService.GetAllMaillist(SPContext.Current.Web);
                var maillist = EmailListService.GetAllEmailListItems(SPContext.Current.Site.RootWeb);
                if (maillist != null)
                {
                    foreach (var item in maillist)
                    {
                        //cblEmailList.Items.Add(new ListItem() { Text = item.Value, Value = item.Value });
                        cblEmailList.Items.Add(new ListItem() { Text = item.Title, Value = item.AllEmails });
                    }
                }
            }

            if (!IsPostBack)
            {
                _displayActionData();
            }
            base.OnLoad(e);
        }
        public List<string> GetMaillist()
        {
            List<string> list = new List<string>();

            foreach (ListItem item in cblEmailList.Items)
            {
                if (item.Selected)
                    list.Add(item.Value);    
            }
            return list;
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

            return new SendEmailToStaticAddressesSettings()
            {
                EmailAddress = txtEmail.Text.Trim(),
                AttachTaskLink = ckbAttachTaskLink.Checked,
                EmailTemplateName = EmailSelector.TemplateName,
                EmailTemplateUrl = EmailSelector.TemplateUrl,
                StaticUsers = staticUsers
            };
        }
        private void _displayActionData()
        {
            if (Action == null) return;
            

            SendEmailToStaticAddressesSettings savedAction = (SendEmailToStaticAddressesSettings)Action;
            txtEmail.Text = savedAction.EmailAddress;
            ckbAttachTaskLink.Checked = savedAction.AttachTaskLink;
            EmailSelector.TemplateName = savedAction.EmailTemplateName;
            EmailSelector.TemplateUrl = savedAction.EmailTemplateUrl;

            peSpecificUsersGroups.CommaSeparatedAccounts = string.Join(",", savedAction.StaticUsers.ToArray());
        }
    }
}
