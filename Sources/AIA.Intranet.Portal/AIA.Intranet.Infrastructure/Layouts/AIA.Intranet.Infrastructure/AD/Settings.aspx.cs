using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Infrastructure.Utilities;
using AIA.Intranet.Infrastructure.Models;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class Settings : AdministrationPage
    {
        
        protected RadioButton rbImpersonation;
        protected RadioButton rbSpecificUser;
        protected TextBox txtUsername;
        protected TextBox txtPassword;
        protected Label ErrorMessage;
        protected Label lblMessage;
        protected TextBox txtLDAP;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSavedData();
            }
            lblMessage.Visible = false;
            ErrorMessage.Text = string.Empty;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                ADSetting settings = SharepointHelper.GetADSetting();
                if (settings == null)
                {
                    settings = new ADSetting();
                }


                settings.IsImpersonate = false; //rbImpersonation.Checked;
                settings.UserName = txtUsername.Text;
                settings.LDAP = txtLDAP.Text;


                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    string passWord = CryptoHelper.Encrypt(txtPassword.Text);
                    settings.Password = passWord;
                }

                //ADHelper hepper = new ADHelper();
                //settings.Domain = hepper.GetDomainName();

                SPContext.Current.Web.SetCustomSettings<ADSetting>(IOfficeFeatures.Infrastructure, settings);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error when save setting: " + ex.Message;
                System.Diagnostics.Debug.Write(ex);
                return;
            }

            lblMessage.Visible = true;

            ADHelper helper = new ADHelper();
            bool isAuthenticate = helper.IsUserAuthenticate(txtUsername.Text, txtPassword.Text);
            if (!isAuthenticate)
            {
                ErrorMessage.Text = "Warning: The username or password you entered is incorrect or this account does not have permission to access Active Directory.";
                return;
            }
        }

        protected void ManageGroup_Click(object sender, EventArgs e)
        {
            Response.Redirect("AllGroups.aspx");
        }
        

        private void LoadSavedData()
        {
            ADSetting settings = SharepointHelper.GetADSetting();
            if (settings != null)
            {
                rbImpersonation.Checked = false; // settings.IsImpersonate;
                //rbSpecificUser.Checked = !rbImpersonation.Checked;
                txtUsername.Text = settings.UserName;
                txtPassword.Text = settings.Password;
            }
        }
    }
}