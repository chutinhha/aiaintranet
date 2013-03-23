using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Model.Infrastructure;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class NotificationSettingControl : UserControl
    {
        public bool ReadOnly { get; set; }
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

        public EmailTemplateSelector EmailSelector
        {
            get
            {
                return this.notifyEmail as EmailTemplateSelector;
            }
        }
        public NotificationSettings Setting { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreloadData();
                ShowSetting();
                SetControlStatus(this, ReadOnly);
                btnEdit.Enabled = true;
                
            }
        }

        private void SetControlStatus(Control control, bool disabled){
            
            if (control.Controls != null)
            {
                foreach (Control item in control.Controls)
                {
                    SetControlStatus(item, disabled);
                }
            }
            control.SetProperty("Enabled", !disabled);
            control.SetProperty("ReadOnly", disabled);
        }

        private void PreloadData()
        {
            List<SPField> fields = new List<SPField>();
            if (this.ContentType != null)
            {
                fields = this.ContentType.Fields.Cast<SPField>().Where(p => p.Type == SPFieldType.User ||
                                                                         p.TypeAsString == "AssignmentField" ||
                                                                          p.TypeAsString == "LookupFieldWithPicker" ||
                                                                          p.TypeAsString == "LookupFieldWithPickerMulti").ToList();
            }
            else
            {

                        fields = this.CurrentList.Fields.Cast<SPField>().Where(p => p.Type == SPFieldType.User ||
                                                                          p.TypeAsString == "AssignmentField" ||
                                                                          p.TypeAsString == "LookupFieldWithPicker" ||
                                                                          p.TypeAsString == "LookupFieldWithPickerMulti").ToList();
            
            }
            

            lstColumns.Items.Clear();
            lstCCMetadata.Items.Clear();

            foreach (var item in fields)
            {
              
                lstCCMetadata.Items.Add(new  ListItem()
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                });

                lstColumns.Items.Add(new ListItem()
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                });
            }

            SPList mailList = CCIUtility.GetListFromURL(Constants.EMAIL_LIST_URL, SPContext.Current.Site.RootWeb);
            SPListItemCollection maillistItems = mailList.Items;
            foreach (SPListItem mailItem in maillistItems)
            {
                MultiLookupPicker.AddItem(mailItem[SPBuiltInFieldId.Title].ToString(), mailItem[SPBuiltInFieldId.Title].ToString(), "", "");
            }

            //var maillist = EmailListService.GetAllEmailListItems(SPContext.Current.Site.RootWeb);
            //foreach (var item in maillist)
            //{
            //    MultiLookupPicker.AddItem(item.Title, item.Title, "", "");
            //}
            
        }

        private void ShowSetting()
        {
            
            if (Setting == null) return;

            chkEnable.Checked = Setting.Enable;
            EmailSelector.Value = Setting.Template;
            chkFirstCheckIn.Checked = Setting.RunOnFirstCheckIn;
            chkOnChange.Checked = Setting.RunOnChanged;
            chkOnCreate.Checked = Setting.RunOnCreated;
            chkAllUsers.Checked = Setting.SendToAll;
            chkSpecifyUsers.Checked = Setting.SendToSpecificUsers;
            chkMetadata.Checked = Setting.SendToSelectedColumns;

            if (Setting.SendToSelectedColumns)
            {
                foreach (ListItem item in lstColumns.Items)
                {
                    item.Selected = Setting.SelectedColumns.Contains(item.Value);
                }
            }

            if (Setting.SendToSpecificUsers)
            {
                BindPeoplePicker(ppUsers, Setting.SelectedUserOrGroups);
            }
            
            foreach (ListItem item in lstCCMetadata.Items)
            {
                item.Selected = Setting.CCColumns.Contains(item.Value);
            }
            foreach (var item in Setting.Maillists)
            {
                MultiLookupPicker.AddSelectedItem(item, item);
            }
            chkMaillist.Checked = Setting.SendToMaillist;

            BindPeoplePicker(ppInfomUsers, Setting.CCUserOrGroups);

            string url = Request.RawUrl;
            url = url.Replace("NotificationSettings", "AddNotificationSettings");
            url +="&SID="+ Setting.Id;
            string scripblock=string.Format("javascript:OpenPopUpPageWithTitle('{0}',RefreshOnDialogClose,750,500,'Edit setting');return false", url);

            btnEdit.Attributes.Add("onclick", scripblock);
            //btnDelete.Visible = true;
        }
        private void BindPeoplePicker(PeopleEditor ppEditor, List<string> list)
        {
            string users = "";
            System.Collections.ArrayList entityArrayList = new System.Collections.ArrayList();
            PickerEntity entity = new PickerEntity();
            foreach (var item in list)
            {
                entity.Key = item;
                // this can be omitted if you're sure of what you are doing
                entity = ppEditor.ValidateEntity(entity);
                entityArrayList.Add(entity);
                users += item + ",";
            }

            //ppEditor.UpdateEntities(entityArrayList);
            if (list.Count > 0)
                ppEditor.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
        }

    }
}
