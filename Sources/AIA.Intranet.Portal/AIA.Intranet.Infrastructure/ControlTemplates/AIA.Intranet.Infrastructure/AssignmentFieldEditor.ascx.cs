using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using AIA.Intranet.Infrastructure.CustomFields;
using AIA.Intranet.Common.Helpers;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class AssignmentFieldEditor : UserControl, IFieldEditor
    {
        private bool sendNotifyEmail;
        private string webId;
        private string listId;
        private string columnName;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ddlWebs.SelectedIndexChanged += new EventHandler(ddlWebs_SelectedIndexChanged);
            this.ddlLists.SelectedIndexChanged += new EventHandler(ddlLists_SelectedIndexChanged);
        }

        void ddlWebs_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SPWeb spWeb = SPContext.Current.Site.OpenWeb(new Guid(ddlWebs.SelectedValue)))
            {
                this.UpdateListView(spWeb);
                try
                {
                    if (!String.IsNullOrEmpty(listId))
                    {
                        this.ddlLists.SelectedIndex = ddlLists.Items.IndexOf(ddlLists.Items.FindByValue(listId));
                    }
                }
                catch (Exception ex)
                {
                    CCIUtility.LogError("There is no lists matches " + listId + " in " + ddlWebs.SelectedItem.Text + ".", "Assignment Field");
                }
                
                this.UpdateColumnView(spWeb);
                try
                {
                    if (!String.IsNullOrEmpty(columnName))
                    {
                        this.ddlColumns.SelectedIndex = ddlColumns.Items.IndexOf(ddlColumns.Items.FindByValue(columnName));
                    }
                }
                catch (Exception ex)
                {
                    CCIUtility.LogError("There is no field columns matches " + columnName + " in " + ddlLists.SelectedItem.Text + " list.", "Assignment Field");
                }
            }
        }

        void ddlLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SPWeb spWeb = SPContext.Current.Site.OpenWeb(new Guid(ddlWebs.SelectedValue)))
            {
                this.UpdateColumnView(spWeb);

                try
                {
                    if (!String.IsNullOrEmpty(columnName))
                    {
                        this.ddlColumns.SelectedIndex = ddlColumns.Items.IndexOf(ddlColumns.Items.FindByValue(columnName));
                    }
                }
                catch (Exception ex)
                {
                    CCIUtility.LogError("There is no field columns matches " + columnName + " in " + ddlLists.SelectedItem.Text + " list.", "Assignment Field");
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (!IsPostBack)
            {
                chkSendNotifyEmail.Checked = sendNotifyEmail;


                using (SPWeb spWeb = !string.IsNullOrEmpty(webId) ? SPContext.Current.Site.OpenWeb(new Guid(webId)) : SPContext.Current.Site.OpenWeb())
                {
                    this.UpdateWebView();
                    if (!String.IsNullOrEmpty(webId))
                    {
                        this.ddlWebs.SelectedIndex = ddlWebs.Items.IndexOf(ddlWebs.Items.FindByValue(webId));
                    }
                    else
                    {
                        this.ddlWebs.SelectedIndex = 0;
                    }

                    this.UpdateListView(spWeb);
                    if (!String.IsNullOrEmpty(listId))
                    {
                        this.ddlLists.SelectedIndex = ddlLists.Items.IndexOf(ddlLists.Items.FindByValue(listId));
                    }
                    else
                    {
                        this.ddlLists.SelectedIndex = 0;
                    }

                    this.UpdateColumnView(spWeb);
                    if (!String.IsNullOrEmpty(columnName))
                    {
                        this.ddlColumns.SelectedIndex = ddlColumns.Items.IndexOf(ddlColumns.Items.FindByValue(columnName));
                    }
                    else
                    {
                        this.ddlColumns.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UpdateWebView()
        {
            this.ddlWebs.Items.Clear();

            foreach (SPWeb web in SPContext.Current.Site.AllWebs)
            {
                ddlWebs.Items.Add(new ListItem(web.Url, web.ID.ToString()));
                web.Dispose();
            }

            //this.ddlLists.DataSource = spWeb.Lists;
            //this.ddlLists.DataTextField = "Title";
            //this.ddlLists.DataValueField = "ID";
            //this.ddlLists.DataBind();
        }

        private void UpdateListView(SPWeb spWeb)
        {
            this.ddlLists.Items.Clear();

            this.ddlLists.DataSource = spWeb.Lists;
            this.ddlLists.DataTextField = "Title";
            this.ddlLists.DataValueField = "ID";
            this.ddlLists.DataBind();
        }

        private void UpdateColumnView(SPWeb spWeb)
        {
            string listId = this.ddlLists.SelectedValue;
            SPList list = spWeb.Lists[new Guid(listId)];
            ControlHelper.LoadFieldsToDropdown(this.ddlColumns, list.Fields, false);
        }

        public bool DisplayAsNewSection
        {
            get { return true; }
        }
        public void InitializeWithField(SPField field)
        {
            AssignmentField myField = field as AssignmentField;

            if (myField != null)
            {
                string webUrl = !string.IsNullOrEmpty(field.GetProperty("WebId")) ? field.GetProperty("WebId") : string.Empty;
                string list = myField.GetProperty("List");
                string showField = myField.GetProperty("ShowField");

                string fieldWebId = string.Empty;
                string fieldListId = string.Empty;
                string columnId = string.Empty;

                if (!string.IsNullOrEmpty(list))
                {

                        using (SPWeb spWeb = SPContext.Current.Site.OpenWeb(webUrl))
                        {
                            //cannot use below command in subsites when the list is in rootsite
                            //SPList refList = SPContext.Current.Web.GetList(SPUrlUtility.CombineUrl(SPContext.Current.Web.ServerRelativeUrl, list));

                            fieldWebId = spWeb.ID.ToString();

                            SPList refList = CCIUtility.GetListFromURL(list, spWeb);

                            fieldListId = refList.ID.ToString();

                            if (!string.IsNullOrEmpty(showField))
                            {
                                SPField refField = refList.Fields.GetFieldByInternalName(showField);
                                columnId = refField.Id.ToString();
                            }
                        }
                }

                this.sendNotifyEmail = string.IsNullOrEmpty(myField.SendEmailNotify)? false: Convert.ToBoolean( myField.SendEmailNotify) ;
                this.webId = string.IsNullOrEmpty(myField.WebId) ? fieldWebId : myField.WebId;
                this.listId = string.IsNullOrEmpty(myField.ListId) ? fieldListId : myField.ListId;
                this.columnName = string.IsNullOrEmpty(myField.ColumnName) ? columnId : myField.ColumnName;
            }
        }

        public void OnSaveChange(Microsoft.SharePoint.SPField field, bool isNewField)
        {
            //throw new NotImplementedException();
            bool notify = this.chkSendNotifyEmail.Checked;
            string web = this.ddlWebs.SelectedValue;
            string list = this.ddlLists.SelectedValue;
            string column = this.ddlColumns.SelectedValue;

            //string value = "//test/";
            AssignmentField myField = field as AssignmentField;
            if (isNewField)
            {
                myField.UpdateCustomProperty(Constants.Infrastructure.SendNotifyEmailProperty, notify.ToString());
                myField.UpdateCustomProperty(Constants.Infrastructure.WebIdProperty, web);
                myField.UpdateCustomProperty(Constants.Infrastructure.ListIdProperty, list);
                myField.UpdateCustomProperty(Constants.Infrastructure.ColumnNameProperty, column);
            }
            else
            {
                myField.UpdateCustomProperty(Constants.Infrastructure.SendNotifyEmailProperty, notify.ToString());
                myField.UpdateCustomProperty(Constants.Infrastructure.WebIdProperty, web);
                myField.UpdateCustomProperty(Constants.Infrastructure.ListIdProperty, list);
                myField.UpdateCustomProperty(Constants.Infrastructure.ColumnNameProperty, column);

                myField.SendEmailNotify = chkSendNotifyEmail.Checked.ToString();
                myField.WebId = web;
                myField.ListId = list;
                myField.ColumnName = column;
            }
        }
    }
}
