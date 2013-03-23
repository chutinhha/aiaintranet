using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Infrastructure.Controls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using AIA.Intranet.Infrastructure.Recievers;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class NotificationSettingsPage : LayoutsPageBase
    {
        #region Properties
        protected SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }
        public SPContentType ContentType
        {
            get
            {
                string ctype = Request["ctype"];
                if (string.IsNullOrEmpty(ctype)) return null;

                SPContentTypeId ctypeId = new SPContentTypeId(ctype);
                if (CurrentList != null) return CurrentList.ContentTypes[ctypeId];

                return SPContext.Current.Web.ContentTypes[ctypeId];

            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }
      
        #endregion

        protected override void OnInit(EventArgs e)
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            rptSettings.ItemDataBound += new RepeaterItemEventHandler(rptSettings_ItemDataBound);
            base.OnInit(e);
        }

        void rptSettings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var control = e.Item.FindControl("ctlSetting") as NotificationSettingControl;
                control.Setting = e.Item.DataItem as NotificationSettings;
                control.ReadOnly = true;
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            string url = Request.RawUrl;
            url = url.Replace("NotificationSettings", "AddNotificationSettings");
            Response.Redirect(url);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            BackToPreviousPage();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            this.CurrentList.RemoveCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
            BackToPreviousPage();
        }

        private void BackToPreviousPage()
        {
            SPUtility.Redirect(this.SourceUrl, SPRedirectFlags.Default, this.Context);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //NotificationSettings settings = GetUserInput();

            //if(ContentType!= null){
            //    ContentType.SetCustomSettings<NotificationSettings>(IOfficeFeatures.Infrastructure, settings);
            //}
            //else
            //{
            //    this.CurrentList.SetCustomSettings<NotificationSettings>(IOfficeFeatures.Infrastructure, settings);
            //}
            
            //RegisterEventReciever(settings);

            BackToPreviousPage();
        }

        private void RegisterEventReciever(NotificationSettings settings)
        {
            if (settings.RunOnCreated)
            {
                if (CurrentList != null)
                {
                    this.CurrentList.EnsureEventReciever(typeof(NotificationReciever), SPEventReceiverType.ItemAdded);
                }
                else
                {
                    this.ContentType.EnsureEventReciever(typeof(NotificationReciever), SPEventReceiverType.ItemAdded);
                }
            }

            if (settings.RunOnChanged)
            {
                if (CurrentList != null)
                {
                    this.CurrentList.EnsureEventReciever(typeof(NotificationReciever), SPEventReceiverType.ItemUpdated);
                }
                else
                {
                    this.ContentType.EnsureEventReciever(typeof(NotificationReciever), SPEventReceiverType.ItemUpdated);
                }
            }
        }

      

        private NotificationSettings GetUserInput()
        {
          
            return null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.RawUrl;
            url = url.Replace("NotificationSettings", "AddNotificationSettings");

            string scripblock=string.Format("javascript:OpenPopUpPageWithTitle('{0}',RefreshOnDialogClose,750,500,'Add new setting');return false", url);

            btnAdd.Attributes.Add("onclick", scripblock);

            if (!IsPostBack)
            {
                
                DisplaySettings();
            }
        }

        private void PreloadData()
        {
           
        }

        private void DisplaySettings()
        {
            //if (CurrentList == null) return;
            NotificationSettingsCollection setting = new NotificationSettingsCollection();
            if (CurrentList != null)
            {
                setting = CurrentList.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
            }
            if (ContentType != null)
            {
                setting = ContentType.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
            }
            if(setting == null) return;
            rptSettings.DataSource = setting.Settings;
            rptSettings.DataBind();

            btnDelete.Visible = true;

        }

       


    }
}
