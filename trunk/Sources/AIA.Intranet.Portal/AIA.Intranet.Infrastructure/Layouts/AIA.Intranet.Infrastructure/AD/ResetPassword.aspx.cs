using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using System.Collections.ObjectModel;
using AIA.Intranet.Infrastructure.Models;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using AIA.Intranet.Infrastructure.Utilities;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class ResetPassword : LayoutsPageBase
    {
        protected TextBox txtPassword;
        protected TextBox txtConfirmPassword;
        protected Label lblErrorMessage;
        protected Label lblPassword;
        protected Label lblConfirmPassword;
        protected Label lblResetPasswordSuccess;

        protected Button btnSave;
        protected Button btnCancel;


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!SharepointHelper.HasPermissions(this.Web, new string[] { "Full Control", "Design" }))
            //{
            //    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
            //}
            
            if (!IsPostBack)
            {
               
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;

            try
            {
                ValidatePassword();
                string userDn = Request.QueryString["userDn"];
                userDn = HttpUtility.UrlDecode(userDn);
                string newPassword = txtPassword.Text;
                ADHelper helper = new ADHelper();
                helper.ResetPassword(userDn,newPassword);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                return;
            }

            SetViewToSuccessState();
        }

        private void SetViewToSuccessState()
        {
            lblResetPasswordSuccess.Visible = true;
            txtPassword.Visible = false;
            txtConfirmPassword.Visible = false;
            lblPassword.Visible = false;
            lblConfirmPassword.Visible = false;
            btnSave.Visible = false;
            btnCancel.Text = "Close";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }


        private void ValidatePassword()
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                throw new Exception("Please enter password.");
            }
            if (!string.Equals(txtPassword.Text, txtConfirmPassword.Text))
            {
                throw new Exception("The passwords do not match.");
            }

        }

        
    }
}