using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class RedirectSettingsPage : AdministrationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowSettings();
            }

        }

        private void ShowSettings()
        {
            var settings = Web.GetCustomSettings<RedirectSettings>(IOfficeFeatures.Infrastructure,false);
            if (settings != null)
            {
                chkEnable.Checked = settings.Enable;
                radShowError.Checked = settings.ShowError;
                txtErrorMessage.Text = settings.ErrorMessage;
                radHomePage.Checked = settings.RedirectToHP;

                radToUrl.Checked = settings.RedirectToUrl;
                txtUrl.Text = settings.TargetUrl;
                btnDelete.Visible = true;

            }
        }
        protected override void OnInit(EventArgs e)
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            base.OnInit(e);
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            Web.RemoveCustomSettings<RedirectSettings>(IOfficeFeatures.Infrastructure);
            GoToPreviousPage();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            RedirectSettings setting = new RedirectSettings()
            {

                Enable = chkEnable.Checked,
                ErrorMessage = txtErrorMessage.Text,
                RedirectToHP = radHomePage.Checked,
                RedirectToUrl = radToUrl.Checked,
                ShowError = radShowError.Checked,
                TargetUrl = txtUrl.Text

            };

            Web.SetCustomSettings<RedirectSettings>(IOfficeFeatures.Infrastructure, setting);

            GoToPreviousPage();
        }
    }
}
