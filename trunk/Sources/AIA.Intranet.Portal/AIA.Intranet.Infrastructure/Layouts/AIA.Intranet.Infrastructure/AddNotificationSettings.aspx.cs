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
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Search;
using System.Web.UI;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class AddNotificationSettings : LayoutsPageBase
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
        protected EmailTemplateSelector EmailSelector
        {
             get
            {
                return this.notifyEmail as EmailTemplateSelector;
            }
        }
        #endregion
        private void ShowOperators()
        {
            foreach (Control controls in listFieldsContainer.Controls)
            {
                FormField ffContentItem = (FormField)controls.FindControl("ffContent");
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");
                dropdownOperators.Items.Clear();
                List<Operators> OperatorsGets = OperatorsHelper.GetOperators(ffContentItem.Field.Type);
                dropdownOperators.Items.Add(new ListItem(Constants.NONE_VALUE));
                foreach (Operators OperatorGet in OperatorsGets)
                {
                    dropdownOperators.Items.Add(new ListItem(Constants.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);


            listFieldsContainer.ExcludeFields = "Name";

                foreach (SPField field in CurrentList.Fields)
                {

                    if (!field.Hidden && !field.ReadOnlyField && !this.listFieldsContainer.Fields.ContainFieldId(field.Id))
                    {

                        switch (field.Type)
                        {
                            case SPFieldType.File:
                            case SPFieldType.URL:
                            case SPFieldType.Attachments:
                            case SPFieldType.AllDayEvent:
                                continue;

                            case SPFieldType.Note:
                                SPFieldMultiLineText textField = (SPFieldMultiLineText)field;
                                if (textField.RichText == true)
                                    continue;
                                break;
                        }

                        addCustomField(field);
                    }
                }
            

            base.OnInit(e);
        }
        private void addCustomField(SPField field)
        {
            TemplateContainer child = new TemplateContainer();
            FieldInfo fiFieldName = typeof(TemplateContainer).GetField("m_fieldName", BindingFlags.NonPublic | BindingFlags.Instance);
            fiFieldName.SetValue(child, field.InternalName);

            FieldInfo fiControlMode = typeof(TemplateContainer).GetField("m_renderMode", BindingFlags.NonPublic | BindingFlags.Instance);
            fiControlMode.SetValue(child, listFieldsContainer.ControlMode);
            listFieldsContainer.Controls.Add(child);
            listFieldsContainer.CustomTemplate.InstantiateIn(child);
        }



        void btnCancel_Click(object sender, EventArgs e)
        {
            BackToPreviousPage();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            var existingSetting = new NotificationSettingsCollection();


              string sid = Request["SID"];
              if (!string.IsNullOrEmpty(sid))
              {
                  if (ContentType != null)
                  {
                      existingSetting = ContentType.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
                      var existed = existingSetting.Settings.FirstOrDefault(p => p.Id == new Guid(sid));

                      existingSetting.Settings.Remove(existed);
                      ContentType.SetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure, existingSetting);
                  }
                  else
                  {
                      existingSetting = CurrentList.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
                      var existed = existingSetting.Settings.FirstOrDefault(p => p.Id == new Guid(sid));

                      existingSetting.Settings.Remove(existed);
                      CurrentList.SetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure, existingSetting);
                  }
              }
            
            BackToPreviousPage();
        }

        private void BackToPreviousPage()
        {
            if(!string.IsNullOrEmpty(Request["IsDlg"])){
            
                this.Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                this.Context.Response.End();
            }
            else{
            
            
            SPUtility.Redirect(this.SourceUrl, SPRedirectFlags.Default, this.Context);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            NotificationSettings settings = GetUserInput();

            if(ContentType!= null){
                var existingSetting = ContentType.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);

                if (existingSetting == null) existingSetting = new NotificationSettingsCollection();
                string sid = Request["SID"];
                if (string.IsNullOrEmpty(sid))
                {
                    settings.Id = Guid.NewGuid();

                    existingSetting.Settings.Add(settings);
                }
                else
                {
                    var existed = existingSetting.Settings.FirstOrDefault(p => p.Id == new Guid(sid));
                    settings.Id = existed.Id;
                    existed = settings;
                    

                }
                ContentType.SetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure, existingSetting);
            }
            else
            {
                var existingSetting = CurrentList.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);

                if (existingSetting == null) existingSetting = new NotificationSettingsCollection();
                string sid = Request["SID"];
                if (string.IsNullOrEmpty(sid))
                {
                    settings.Id = Guid.NewGuid();

                    existingSetting.Settings.Add(settings);
                }
                else
                {
                    var existed = existingSetting.Settings.FirstOrDefault(p => p.Id == new Guid(sid));
                    int index = existingSetting.Settings.IndexOf(existed);
                    settings.Id = existed.Id;
                    existingSetting.Settings.RemoveAt(index);
                    existingSetting.Settings.Insert(index, settings);
                    //existed.SyncProperties(settings);
                }
                CurrentList.SetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure, existingSetting);
            }

            //Export xml setting
            //string abce = AIA.Intranet.Common.Helpers.SerializationHelper.SerializeToXml(settings);
            
            RegisterEventReciever(settings);

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
            NotificationSettings input = new NotificationSettings() 
            {
                Enable = chkEnable.Checked,
                Template = EmailSelector.Value,
                RunOnCreated = chkOnCreate.Checked,
                RunOnChanged = chkOnChange.Checked,
                RunOnFirstCheckIn = chkFirstCheckIn.Checked,
                SendToAll = chkAllUsers.Checked,
                SendToSpecificUsers = chkSpecifyUsers.Checked,
                SendToSelectedColumns = chkMetadata.Checked,
                SendToMaillist = chkMaillist.Checked,
                EnableCondition = chkCondition.Enabled,
                
            };

            input.Conditions = GetConditions();
            foreach (PickerEntity item in ppInfomUsers.ResolvedEntities)
            {
                input.CCUserOrGroups.Add(item.Key);
            }

            if (chkSpecifyUsers.Checked)
            {
                foreach (PickerEntity  item in ppUsers.ResolvedEntities)
                {
                    input.SelectedUserOrGroups.Add(item.Key);
                }
            }

            if (chkMetadata.Checked)
            {
                input.SelectedColumns = lstColumns.Items.Cast<ListItem>()
                                                             .Where(p => p.Selected)
                                                             .Select(p => p.Value)
                                                             .ToList();
               
            }

            input.CCColumns = lstCCMetadata.Items.Cast<ListItem>()
                                                            .Where(p => p.Selected)
                                                            .Select(p => p.Value)
                                                            .ToList();

            input.Maillists = MultiLookupPicker.SelectedIds.Cast<string>().ToList();

            return input;
        }


        private List<Criteria> GetConditions()
        {
            List<Criteria> criterias = new List<Criteria>();
            //set criterias
            foreach (Control controls in listFieldsContainer.Controls)
            {
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");
                if (dropdownOperators.SelectedIndex != -1 && dropdownOperators.SelectedValue != Constants.NONE_VALUE)
                {
                    FormField ffContentItem = (FormField)controls.FindControl("ffContent");
                    Criteria criteria = new Criteria();
                    criteria.Operator = (Operators)Enum.Parse(typeof(Operators), dropdownOperators.SelectedValue);
                    criteria.FieldId = ffContentItem.Field.Id.ToString();
                    if (ffContentItem.Value != null)
                    {
                        criteria.Value = ffContentItem.Value.ToString();
                        criterias.Add(criteria);
                    }
                }
            }
            return criterias;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreloadData();
                ShowOperators();
                DisplaySettings();
            }
        }

        private void PreloadData()
        {

            var fields = this.CurrentList.Fields.Cast<SPField>().Where(p=>p.Type == SPFieldType.User ||
                                                                          p.TypeAsString == "AssignmentField" ||
                                                                          p.TypeAsString == "LookupFieldWithPicker" ||
                                                                          p.TypeAsString == "LookupFieldWithPickerMulti").ToList();
            lstColumns.Items.Clear();
            lstCCMetadata.Items.Clear();

            foreach (var item in fields)
            {
                var listItem = new ListItem() { 
                Text = item.Title,
                Value = item.Id.ToString()
                };
                lstCCMetadata.Items.Add(listItem);
                lstColumns.Items.Add(listItem.Clone<ListItem>());
                
            }

            SPList mailList = CCIUtility.GetListFromURL(Constants.EMAIL_LIST_URL, SPContext.Current.Site.RootWeb);
            SPListItemCollection maillistItems = mailList.Items;
            foreach(SPListItem mailItem in maillistItems)
            {
                MultiLookupPicker.AddItem(mailItem[SPBuiltInFieldId.Title].ToString(), mailItem[SPBuiltInFieldId.Title].ToString(), "", "");
            }

            //var maillist = EmailListService.GetAllEmailListItems(SPContext.Current.Site.RootWeb);
            //foreach (var item in maillist)
            //{
            //    MultiLookupPicker.AddItem(item.Title, item.Title, "", "");
            //}
        }

        private void DisplaySettings()
        {

            //if (CurrentList == null) return;

            var settings = new NotificationSettingsCollection();


            if (ContentType != null)
            {
                settings = ContentType.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
            }
            else
            {
                settings = CurrentList.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);
            }
            if (settings == null) return;

            var setting = settings.Settings.FirstOrDefault(p => p.Id.ToString() == Request["SID"]);

            if (setting == null) return;
            chkEnable.Checked = setting.Enable;
            EmailSelector.Value = setting.Template;
            chkFirstCheckIn.Checked = setting.RunOnFirstCheckIn;
            chkOnChange.Checked = setting.RunOnChanged;
            chkOnCreate.Checked = setting.RunOnCreated;
            chkAllUsers.Checked = setting.SendToAll;
            chkSpecifyUsers.Checked = setting.SendToSpecificUsers;
            chkMetadata.Checked = setting.SendToSelectedColumns;
            chkMaillist.Checked = setting.SendToMaillist;
            chkCondition.Checked = setting.EnableCondition;
            if (setting.SendToSelectedColumns)
            {
                foreach (ListItem item in lstColumns.Items)
                {
                    item.Selected = setting.SelectedColumns.Contains(item.Value);
                }
            }

            foreach (ListItem item in lstCCMetadata.Items)
            {
                item.Selected = setting.CCColumns.Contains(item.Value);
            }


            if (setting.SendToSpecificUsers)
            {
                BindPeoplePicker(ppUsers, setting.SelectedUserOrGroups);
            }
            BindPeoplePicker(ppInfomUsers, setting.CCUserOrGroups);
            btnDelete.Visible = true;
            foreach (var item in setting.Maillists)
            {
                MultiLookupPicker.AddSelectedItem(item, item);
            }

            foreach (Control controls in listFieldsContainer.Controls)
            {
                FormField ffContentItem = (FormField)controls.FindControl("ffContent");
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");

                Criteria criteria = setting.Conditions.FirstOrDefault(c => c.FieldId == ffContentItem.Field.Id.ToString());
                if (criteria != null)
                {
                    // set default value to dropdown operators 
                    dropdownOperators.SelectedValue = criteria.Operator.ToString();
                    switch (ffContentItem.Field.Type)
                    {
                        case SPFieldType.Choice:
                            {
                                IEnumerable<DropDownList> drops = ffContentItem.Controls.OfTypeRecursive<DropDownList>();
                                if (drops.Count<DropDownList>() > 0)
                                {
                                    ffContentItem.Value = criteria.Value;
                                }

                                IEnumerable<RadioButton> radios = ffContentItem.Controls.OfTypeRecursive<RadioButton>();
                                foreach (RadioButton radio in radios)
                                {
                                    if (radio.Text == criteria.Value)
                                        radio.Checked = true;
                                }
                                break;
                            }

                        case SPFieldType.MultiChoice:
                            {
                                IEnumerable<CheckBox> checks = ffContentItem.Controls.OfTypeRecursive<CheckBox>();
                                foreach (CheckBox check in checks)
                                {
                                    SPFieldMultiChoiceValue multiVal = new SPFieldMultiChoiceValue(criteria.Value);
                                    for (int i = 0; i < multiVal.Count; i++)
                                        if (check.Text.ToLower() == multiVal[i].ToLower()) check.Checked = true;
                                }

                                break;
                            }

                        case SPFieldType.Boolean:
                            {
                                ffContentItem.Value = Convert.ToBoolean(criteria.Value);
                                break;
                            }

                        case (SPFieldType.DateTime):
                            {
                                ffContentItem.Value = Convert.ToDateTime(criteria.Value);
                                break;
                            }

                        case (SPFieldType.Lookup):
                            {
                                IEnumerable<TextBox> txtLookups = ffContentItem.Controls.OfTypeRecursive<TextBox>();
                                if (txtLookups.Count<TextBox>() > 0)
                                {
                                    TextBox txtLookup = txtLookups.First<TextBox>();
                                    if (txtLookup.Attributes["optHid"] != null)
                                    {
                                        string clientScript = " var hidden = document.getElementById('" + txtLookup.Attributes["optHid"] + "') \r\n";
                                        clientScript += " if (hidden != null){ hidden.value=" + criteria.Value + "}";
                                        ScriptPlaceHolder.Text = string.Format("<script type=\"text/javascript\">\r\n {0} \r\n</script>", clientScript);
                                    }
                                }
                                ffContentItem.Value = criteria.Value;
                                break;
                            }

                        //case SPFieldType.Invalid:
                        //    if (string.Compare(ffContentItem.Field.TypeAsString,
                        //   Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                        //    {
                        //        LookupFieldWithPicker fieldLookupWithPicker = (LookupFieldWithPicker)ffContentItem.Field;
                        //        LookupFieldWithPickerControl lookupControl = fieldLookupWithPicker.FieldRenderingControl as LookupFieldWithPickerControl;
                        //        lookupControl.Value = new SPFieldLookupValue(criteria.Value);
                        //        ffContentItem.Value = new SPFieldLookupValue(criteria.Value);
                        //    }
                        //    break;
                        default:
                            {
                                ffContentItem.Value = criteria.Value;
                                break;
                            }
                    }

                }
            }
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
