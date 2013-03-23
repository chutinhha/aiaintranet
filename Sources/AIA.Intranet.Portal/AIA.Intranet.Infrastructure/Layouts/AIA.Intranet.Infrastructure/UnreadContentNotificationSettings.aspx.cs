using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using AIA.Intranet.Infrastructure.Controls;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.Recievers;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class UnreadContentNotificationSettings : LayoutsPageBase
    {
        protected EmailTemplateSelector EmailSelector
        {
            get
            {
                return this.notifyEmail as EmailTemplateSelector;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            chkEnable.CheckedChanged += new EventHandler(chkEnable_CheckedChanged);
            chkEnableSendEmail.CheckedChanged += new EventHandler(chkEnableSendEmail_CheckedChanged);
            chkEnableCreateUnreadTask.CheckedChanged += new EventHandler(chkEnableCreateUnreadTask_CheckedChanged);

            base.OnInit(e);
        }


        void btnDelete_Click(object sender, EventArgs e)
        {
            
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetEnableForControl(false, notifyEmail);
                LoadUnreadContentNotificationSetting();
            }
        }

        void SetEnableForControl(bool isEnable, params WebControl[] controls)
        {
            if (controls.Length > 0)
            {
                foreach (WebControl control in controls)
                {
                    control.Enabled = isEnable;
                }
            }
        }

        void SetEnableForControl(bool isEnable, params UserControl[] controls)
        {
            if (controls.Length > 0)
            {
                foreach (UserControl control in controls)
                {
                    if (control is EmailTemplateSelector)
                    {
                        ((EmailTemplateSelector)control).Enable = isEnable;
                    }
                }
            }
        }

        void SetCheckedForControl(bool isChecked, params WebControl[] controls)
        {
            if (controls.Length > 0)
            {
                foreach (WebControl control in controls)
                {
                    if (control is CheckBox)
                    {
                        ((CheckBox)control).Checked = isChecked;
                    }
                    else if (control is RadioButton)
                    {
                        ((RadioButton)control).Checked = isChecked;
                    }
                }
            }
        }


        void chkEnableCreateUnreadTask_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableForControl(chkEnableCreateUnreadTask.Checked, txtCreateUnreadTask);
        }

        void chkEnableSendEmail_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableForControl(chkEnableSendEmail.Checked, notifyEmail);
        }

        void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            chkEnable_SetEnableForControlWhenCheckedChanged();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            var obj = new UnreadContentNotificationSetting();

            if (chkEnable.Checked)
            {
                obj.Enable = true;
                obj.EnableEmail = chkEnableSendEmail.Checked;
                obj.EnableCreateUnreadTask = chkEnableCreateUnreadTask.Checked;

                if (chkEnableCreateUnreadTask.Checked)
                {
                    obj.TitleFormula = txtCreateUnreadTask.Text;
                }
                else
                {
                    obj.TitleFormula = string.Empty;
                }

                if (chkEnableSendEmail.Checked)
                {
                    obj.Template.Url = EmailSelector.TemplateUrl;
                    obj.Template.Name = EmailSelector.TemplateName;
                }
                else
                {
                    obj.Template.Url = string.Empty;
                    obj.Template.Name = string.Empty;
                }

            }
            else
            {
                obj.Enable = false;
                obj.EnableEmail = false;
                obj.EnableCreateUnreadTask = false;
                obj.TitleFormula = string.Empty;
                obj.Template.Url = string.Empty;
                obj.Template.Name = string.Empty;
            }

            CurrentList.SetCustomSettings<UnreadContentNotificationSetting>(Model.IOfficeFeatures.Infrastructure, obj);
            CurrentList.EnsureEventReciever(typeof(UnreadContentReciever), 10002, SPEventReceiverSynchronization.Synchronous, SPEventReceiverType.ItemAdded);
        }

        public SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }
        void chkEnable_SetEnableForControlWhenCheckedChanged()
        {
            if (chkEnable.Checked)
            {
                SetEnableForControl(true, chkEnableCreateUnreadTask, chkEnableSendEmail);
                if (chkEnableSendEmail.Checked)
                {
                    SetEnableForControl(true, notifyEmail);
                }

                if (chkEnableCreateUnreadTask.Checked)
                {
                    SetEnableForControl(true, txtCreateUnreadTask);
                }
            }
            else
            {
                SetEnableForControl(false, chkEnableCreateUnreadTask, chkEnableSendEmail, txtCreateUnreadTask);
                SetEnableForControl(false, notifyEmail);
            }
        }

        void LoadUnreadContentNotificationSetting()
        {
            var obj = SPContext.Current.List.GetCustomSettings<UnreadContentNotificationSetting>(Model.IOfficeFeatures.Infrastructure);

            if (obj != null)
            {
                chkEnable.Checked = obj.Enable;
                chkEnableSendEmail.Checked = obj.EnableEmail;
                chkEnableCreateUnreadTask.Checked = obj.EnableCreateUnreadTask;
                txtCreateUnreadTask.Text = obj.TitleFormula;
                EmailSelector.TemplateName = obj.Template.Name;
                EmailSelector.TemplateUrl = obj.Template.Url;

                chkEnable_SetEnableForControlWhenCheckedChanged();
            }
            else
            {
                SetCheckedForControl(false, chkEnable,chkEnableSendEmail,chkEnableCreateUnreadTask);
                txtCreateUnreadTask.Text = string.Empty;
                EmailSelector.TemplateName = string.Empty;
                EmailSelector.TemplateUrl = string.Empty;

                SetEnableForControl(false, chkEnableSendEmail, chkEnableCreateUnreadTask, txtCreateUnreadTask);
                SetEnableForControl(false, notifyEmail);
            }
        }
    }
}
