using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Infrastructure.Pages;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class AutoCreationSettingPage : AdministrationPage
    {
        protected override void OnInit(EventArgs e)
        {
            btnMoveRight.Click += new EventHandler(btnMoveRight_Click);
            btnMoveLeft.Click  += new EventHandler(btnMoveLeft_Click);
            ddlGroup.SelectedIndexChanged += new EventHandler(ddlGroup_SelectedIndexChanged);
            btnSave.Click   += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            base.OnInit(e);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            this.CurrentContentType.RemoveCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);
            GoToPreviousPage();
        }
        public override void GoToPreviousPage()
        {

            if (this.CurrentList == null)
            {
                Response.Redirect(SPContext.Current.Web.Url + string.Format("/_layouts/ManageContentType.aspx?ctype={0}", Request["ctype"]));
            }
            else
            {
                Response.Redirect(SPContext.Current.Web.Url + string.Format("/_layouts/ManageContentType.aspx?ctype={0}&List={1}", Request["ctype"], Request["List"]));
            }
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            AutoCreationSettings settings = new AutoCreationSettings()
            {
                EnableCreateList = chkListEnable.Checked,
                ListDefinition = new ListIntanceDefinition()
                {
                    Hidden = chkHidden.Checked,
                    ShowOnQuickLaunch = chkQuicklaunch.Checked,
                    Title = txtListName.Text,
                    Url = txtListUrl.Text,
                    TemplateName = ddlTemplates.SelectedValue,
                    ContentTypes = lstSelected.Items.Cast<ListItem>().Select(p => p.Value).ToList(),
                },
                RunOnChanged = chkOnChange.Checked,
                RunOnCreated = chkOnCreate.Checked,
                RunOnFirstCheckIn = chkFirstCheckIn.Checked,
                UrlFieldName = txtUrlFieldName.Text,
                EnableNavigationUpdate = chkEnableNavigationUpdate.Checked,
                NavigationUpdate = new NavigationUpdateProperties() 
                {
                    Key = txtNavigationKey.Text,
                    Title = txtNavigationName.Text
                }
            };

            this.CurrentContentType.SetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure, settings);
            GoToPreviousPage();
        }

        void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var datasource = SPContext.Current.Web.AvailableContentTypes.Cast<SPContentType>().Where(p => !p.Hidden && p.Group == ddlGroup.SelectedValue).ToList();

            lstSelected.Items.Clear();
            lstAvailable.DataSource = datasource;
            lstAvailable.DataTextField = "Name";
            lstAvailable.DataValueField = "Id";
            lstAvailable.DataBind();
        }

        void btnMoveLeft_Click(object sender, EventArgs e)
        {
            MoveSelect(this.lstSelected, this.lstAvailable);
        }

        void btnMoveRight_Click(object sender, EventArgs e)
        {
            MoveSelect(this.lstAvailable, this.lstSelected);
        }

        private void MoveSelect(System.Web.UI.WebControls.ListBox lstBox1, System.Web.UI.WebControls.ListBox lstBox2)
        {
            var selected = lstBox1.Items.Cast<ListItem>().Where(p => p.Selected).ToArray();
            lstBox2.Items.AddRange(selected);
            foreach (var item in selected)
            {
                lstBox1.Items.Remove(item);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var datasource = SPContext.Current.Web.AvailableContentTypes.Cast<SPContentType>().Where(p => !p.Hidden).ToList();
                lstAvailable.DataSource = datasource;
                lstAvailable.DataTextField = "Name";
                lstAvailable.DataValueField = "Id";
                lstAvailable.DataBind();
                var groups = datasource.Select(p => p.Group).Distinct().ToList();

                ddlGroup.DataSource = groups.Where(p => p != "_Hidden");
                ddlGroup.DataBind();
                var templates = SPContext.Current.Web.ListTemplates.Cast<SPListTemplate>().ToList();
                ddlTemplates.DataSource = templates;
                ddlTemplates.DataTextField = "Name";
                ddlTemplates.DataValueField = "Name";
                ddlTemplates.DataBind();


                var currentSetting = this.CurrentContentType.GetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);
                if (currentSetting != null)
                {
                    chkListEnable.Checked = currentSetting.EnableCreateList;
                    chkFirstCheckIn.Checked = currentSetting.RunOnFirstCheckIn;
                    chkOnChange.Checked = currentSetting.RunOnChanged;
                    chkOnCreate.Checked = currentSetting.RunOnCreated;
                    txtUrlFieldName.Text = currentSetting.UrlFieldName;
                    txtListName.Text = currentSetting.ListDefinition.Title;
                    txtListUrl.Text = currentSetting.ListDefinition.Url;
                    ddlTemplates.SelectedValue = currentSetting.ListDefinition.TemplateName;
                    btnDelete.Visible = true;

                    //Navigation update
                    chkEnableNavigationUpdate.Checked = currentSetting.EnableNavigationUpdate;
                    txtNavigationKey.Text = currentSetting.NavigationUpdate.Key;
                    txtNavigationName.Text = currentSetting.NavigationUpdate.Title;
                }

            }
        }
    }
}
