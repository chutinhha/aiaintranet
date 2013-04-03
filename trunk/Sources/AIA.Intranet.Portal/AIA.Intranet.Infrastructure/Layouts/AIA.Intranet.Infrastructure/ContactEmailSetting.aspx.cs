using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using System.Web;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class ContactEmailSetting : LayoutsPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSettings();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GoToListSettingsPage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            //get the collection of users from people editor
            string users = string.Empty;
            SPFieldUserValueCollection spFieldUserValues = new SPFieldUserValueCollection();
            int index = 0;
            for (index = 0; index <= this.peContactUsers.ResolvedEntities.Count - 1; ++index)
            { 
                 PickerEntity objEntity = (PickerEntity)this.peContactUsers.ResolvedEntities[index];
                 users += objEntity.EntityData["AccountName"] + ",";
            }
            
            SPContext.Current.List.SetCustomProperty(Constants.CONTACT_INTERNAL_EMAIL_PROPERTY, users.TrimEnd(','));
            SPContext.Current.List.SetCustomProperty(Constants.CONTACT_EXTERNAL_EMAIL_PROPERTY, txtContactEmail.Text);
            GoToListSettingsPage();
        }

        private void LoadSettings()
        {
            string emailUser = SPContext.Current.List.GetCustomProperty(Constants.CONTACT_INTERNAL_EMAIL_PROPERTY);
            if (!string.IsNullOrEmpty(emailUser))
            {
                peContactUsers.CommaSeparatedAccounts = emailUser;
            }
            string emailText = SPContext.Current.List.GetCustomProperty(Constants.CONTACT_EXTERNAL_EMAIL_PROPERTY);
            if (!string.IsNullOrEmpty(emailText))
            {
                txtContactEmail.Text = emailText;
            }
        }

        private void GoToListSettingsPage()
        {
            SPUtility.Redirect(SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/listedit.aspx?List={" + SPContext.Current.List.ID.ToString() + "}", SPRedirectFlags.Default, HttpContext.Current);
        }
    }
}
