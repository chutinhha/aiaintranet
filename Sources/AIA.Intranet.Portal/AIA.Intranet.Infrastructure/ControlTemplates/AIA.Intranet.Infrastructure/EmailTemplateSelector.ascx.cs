using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using System.Collections.Generic;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class EmailTemplateSelector : UserControl
    {

        #region properties

        private string templateName;
        public string TemplateUrl
        {
            set { txtTemplateUrl.Text = value; }
            get { return txtTemplateUrl.Text; }
        }
        public string TemplateName
        {
            set
            {
                templateName = value;
            }
            get
            {

                return txtTemplateName.SelectedValue;
            }
        }
        public string ValidationGroup { get; set; }
        public bool Enable
        {
            set
            {
                txtTemplateUrl.Enabled = value;
                txtTemplateName.Enabled = value;
            }
        }
        public List<DropDownList> AssociateControls { get; set; }
        private bool allowNull;
        public bool AllowNull
        {
            set
            {
                //txtTemplateNameValidator.Enabled = !value;
                //txtTemplateUrlValidator.Enabled = !value;
                allowNull = value;
            }
            get
            {
                return allowNull;
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            txtTemplateUrl.TextChanged += new EventHandler(txtTemplateUrl_TextChanged);
            txtTemplateName.ValidationGroup = ValidationGroup;
            txtTemplateNameValidator.ValidationGroup = ValidationGroup;

            txtTemplateNameValidator.Enabled = !allowNull;
            txtTemplateUrlValidator.Enabled = !allowNull;

            BindDropDownSource(txtTemplateName, null);

            base.OnInit(e);
        }

        void txtTemplateUrl_TextChanged(object sender, EventArgs e)
        {
            LoadEmailTemplateItems(txtTemplateUrl.Text, string.Empty);
        }



        private void LoadEmailTemplateItems(string templateUrl, string selectedValue)
        {
            try
            {
                txtTemplateName.Items.Clear();
                ListItemCollection DropdownListSource = new ListItemCollection();
                SPList list = CCIUtility.GetListFromURL(templateUrl);
                SPQuery query = new SPQuery() { Query = "<OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>" };

                SPListItemCollection items = list.GetItems(query);

                DropdownListSource.Insert(0, (new ListItem() { Text = "Select a template", Value = string.Empty }));
                foreach (SPListItem item in items)
                {
                    DropdownListSource.Add(new ListItem() { Text = item["Title"] as string, Value = item["Title"] as string });
                }
                if (DropdownListSource.Count > 0)
                {
                    BindDropDownSource(txtTemplateName, DropdownListSource);
                    if (string.IsNullOrEmpty(selectedValue))
                        txtTemplateName.Items[0].Selected = true;
                    else
                        txtTemplateName.SelectedValue = selectedValue;

                    //Load Extend DropDownList Source
                    if (AssociateControls != null)
                    {
                        foreach (DropDownList ddl in AssociateControls)
                        {
                            BindDropDownSource(ddl, DropdownListSource);
                        }
                    }
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(templateUrl) && allowNull == false)
                {
                    txtTemplateName.Items.Clear();
                    txtTemplateUrlValidator.ErrorMessage = "Cannot get email template from this url";
                    txtTemplateUrlValidator.IsValid = false;
                }
            }


        }

        protected void BindDropDownSource(DropDownList oDropDownList, ListItemCollection oListItemCollection)
        {
            oDropDownList.Items.Clear();
            oDropDownList.DataSource = oListItemCollection;
            oDropDownList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtTemplateUrlValidator.ErrorMessage = "Please enter url of the email template list.";


            if (ViewState["SuperCheat"] == null && !string.IsNullOrEmpty(this.TemplateUrl))
            {
                LoadEmailTemplateItems(this.TemplateUrl, templateName);
            }
            if (ViewState["SuperCheat"] == null) ViewState["SuperCheat"] = true;
        }

        public Model.Infrastructure.EmailTemplateSettings Value
        {
            get
            {

                return new EmailTemplateSettings()
                {
                    Name = TemplateName,
                    Url = TemplateUrl
                };


            }
            set
            {
                EmailTemplateSettings template = value as EmailTemplateSettings;
                TemplateName = template.Name;
                TemplateUrl = template.Url;
            }
        }
    }
}
