using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Search;
using AIA.Intranet.Model.Security;

using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model.Security;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class EditRule : SecuredPageLayout
    {
        #region Fields
        private SPList m_list;
        private string _ruleId = string.Empty;
        private Rule _rule = null;
        private bool _newMode = true; //true : add new ; false: edit
        private SPContentType _contentType;
        #endregion

        #region Constants
        private const string NONE_VALUE = "(None)";
        private const string NONE_OPERATOR_VALUE = "(Not Apply)";
        #endregion

        protected SPList CurrentList
        {
            get
            {
                if (this.m_list == null)
                {
                    string listQS = base.Request.QueryString["List"];
                    if (listQS != null)
                    {
                        this.m_list = base.Web.Lists[new Guid(listQS)];
                    }
                }
                return this.m_list;
            }
        }

        protected SPContentType CurrentContentType
        {
            get
            {
                if (this._contentType == null)
                {
                    string strContentType = base.Request["ContentTypeId"];
                    if (!string.IsNullOrEmpty(strContentType))
                    {
                        this._contentType = this.CurrentList.ContentTypes[new SPContentTypeId(strContentType)];
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(drpContentTypes.SelectedValue))
                            _contentType = this.CurrentList.ContentTypes[new SPContentTypeId(drpContentTypes.SelectedValue)];
                    }
                }
                return this._contentType;
            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }

        #region Event Handlers
        protected override void OnLoad(EventArgs e)
        {
            _rule = getCurrentRule();

            if (_rule != null)
            {
                this.BtnDelete.Enabled = true;
                this.BtnDeleteTop.Enabled = true;
                this.BtnSave.Text = "Save";
                this.BtnSaveTop.Text = "Save";
                _newMode = false;
            }

            if (!IsPostBack)
            {
                string ownerPermission = (_rule == null) ? string.Empty : _rule.OwnerPermission;
                showTypeHandlers();
                showPreserveExistingSecurity();
                showOwnerPermDropDown(ownerPermission);
                showPermissionAssigments();
                showOperators();
                showAvailableContentTypes();
            }
            showSelectContentType();
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {
            RemoveRequiredPropertiesAndExcludeFields();
            if (CurrentContentType != null && CurrentContentType.Id.IsChildOf(SPBuiltInContentTypeId.Folder))
            {
                SPField field = CurrentContentType.Fields["Name"];
                addCustomField(field);
            }
            
            if (CurrentContentType == null)
            {
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
            }
            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["RuleName"])) txtRuleName.Text = Request["RuleName"];
                if (!string.IsNullOrEmpty(Request["DocTypes"])) txtDocumentTypes.Text = Request["DocTypes"];
                string events = Request["Events"];
                if (!string.IsNullOrEmpty(events))
                {
                    chkItemAdded.Checked = events.Contains("ItemAdded");
                    chkAnyUpdate.Checked = events.Contains("AnyUpdate");
                    chkFirstUpdate.Checked = events.Contains("FirstUpdate");
                }
            }
            if (!_newMode)
            {
                showRule(_rule);
            }

            base.CreateChildControls();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (_newMode)
                addNewRule();
            else
                updateRule();

            if (this.SourceUrl != null)
                Response.Redirect(this.SourceUrl);

        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            SecuritySettings setting = this.CurrentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
            Rule rule = setting.Rules.FirstOrDefault(r => r.ID == _ruleId);
            setting.Rules.Remove(rule);
            setting.Rules = reOrderRuleList(setting.Rules);
            this.CurrentList.SetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp, setting);

            if (this.SourceUrl != null)
                Response.Redirect(this.SourceUrl);
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.SourceUrl != null)
                Response.Redirect(this.SourceUrl);
        }
        private int ii = 0;
        protected void repeaterPermissionAssignments_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    ii++;
                
                InputFormSection inputFormSection = item.FindControl("inputFormSection") as InputFormSection;

                InputFormControl inputForm = inputFormSection.FindControl("inputForm") as InputFormControl;

                //load data to available fields
                ListBox lsbAvailabaleFields = (ListBox)inputForm.FindControl("lsbAvailabaleFields");
                ListBox lsbSelectedFields = (ListBox)inputForm.FindControl("lsbSelectedFields");
                loadDataToAvailableFields(lsbAvailabaleFields);

                HiddenField txtPermissionName = (HiddenField)inputForm.FindControl("txtPermissionName");
                SPRoleDefinition roleItem = (SPRoleDefinition)item.DataItem;
                txtPermissionName.Value = roleItem.Name;

                // set value to people editor
                if (_rule == null) return;
                PermissionAssigment permission = _rule.PermissionAssignments.FirstOrDefault(p => p.PermissionLevel == roleItem.Name);
                
                if (permission != null && inputForm!= null)
                {
                    PeopleEditor userEditor = (PeopleEditor)inputForm.FindControl("userEditor");
                    showPeoplePickerUI(userEditor, permission);
                }

                //show data availabe and selected fields
                showAvailableAndSelectedFields(permission, lsbAvailabaleFields, lsbSelectedFields);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void loadDataToAvailableFields(ListBox listFields)
        {
            List<SPField> fields = null;
            if (CurrentContentType != null)
            {
                fields = (List<SPField>)(from SPField field in CurrentContentType.Fields.Cast<SPField>().ToList()
                                         where field.Type == SPFieldType.User
                                         select field).ToList();
            }
            else
            {
                fields = (List<SPField>)(from SPField field in CurrentList.Fields.Cast<SPField>().ToList()
                                         where field.Type == SPFieldType.User
                                         select field).ToList();
            }

            //if (CurrentList.BaseTemplate == SPListTemplateType.DocumentLibrary)
            //{
            //    SPField createdByField = SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Created_x0020_By];
            //    if (!fields.Exists(f => f.Id == createdByField.Id))
            //        fields.Add(createdByField);
            //    SPField modifiedByField = SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Modified_x0020_By];
            //    if (!fields.Exists(f => f.Id == modifiedByField.Id))
            //        fields.Add(modifiedByField);
            //}
            //else
            {
                SPField createdByField = SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Author];
                if (!fields.Exists(f => f.Id == createdByField.Id))
                    fields.Add(createdByField);
                SPField modifiedByField = SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.Editor];
                if (!fields.Exists(f => f.Id == modifiedByField.Id))
                    fields.Add(modifiedByField);
            }

            fields.Sort(delegate(SPField f1, SPField f2) { return f1.Title.CompareTo(f2.Title); });

            listFields.DataSource = fields;
            listFields.DataTextField = "Title";
            listFields.DataValueField = "Id";
            listFields.DataBind();
        }

        private static void showAvailableAndSelectedFields(PermissionAssigment permission, ListBox listAvailableFields, ListBox listSelectedFields)
        {
            if (permission != null && permission.FieldIds != null && permission.FieldIds.Count > 0)
            {
                List<ListItem> originalItems = listAvailableFields.Items.Cast<ListItem>().ToList();
                foreach (ListItem item in originalItems)
                {
                    if (permission.FieldIds.Contains(item.Value))
                    {
                        listSelectedFields.Items.Add(item);
                        listAvailableFields.Items.Remove(item);
                    }
                }
            }
        }
        #endregion

        #region Execute Code
        private void showPermissionAssigments()
        {
            SPWeb web = SPContext.Current.Web;
            var queryRoleDefinitions = from SPRoleDefinition role in web.RoleDefinitions
                                       where role.Hidden == false && role.Name != Constants.EEC.EECOverrideCheckout
                                       select role;
            var data = queryRoleDefinitions.ToList();
            this.repeaterPermissionAssignments.DataSource = data;
            this.repeaterPermissionAssignments.DataBind();
        }

        private void showTypeHandlers()
        {
            chkItemAdded.Checked = (_rule == null) ? false : _rule.RunOnAdded;
            chkFirstUpdate.Checked = (_rule == null) ? false : _rule.RunOnFirstUpdate;
            chkAnyUpdate.Checked = (_rule == null) ? false : _rule.RunOnAnyUpdate;
        }

        private void showPreserveExistingSecurity()
        {
            chkPreserveExistingSecurity.Checked = (_rule == null) ? false : _rule.PreserveExistingSecurity;
        }

        private void showRule(Rule rule)
        {
            if (rule == null) return;
            if (!string.IsNullOrEmpty(Request["RuleName"])) txtRuleName.Text = Request["RuleName"];
            else txtRuleName.Text = rule.Name;

            if (!string.IsNullOrEmpty(Request["DocTypes"])) txtDocumentTypes.Text = Request["DocTypes"];
            else txtDocumentTypes.Text = rule.DocumentTypes;

            string events = Request["Events"];
            if (!string.IsNullOrEmpty(events))
            {
                chkItemAdded.Checked = events.Contains("ItemAdded");
                chkAnyUpdate.Checked = events.Contains("AnyUpdate");
                chkFirstUpdate.Checked = events.Contains("FirstUpdate");
            }

            
            //txtRuleName.Text = Request["RuleName"];
           
            //txtRuleName.Text = Request["DocTypes"];
            List<Criteria> criterias = rule.CriteriaList;

            //load value to field
            foreach (Control controls in listFieldsContainer.Controls)
            {
                FormField ffContentItem = (FormField)controls.FindControl("ffContent");
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");

                Criteria criteria = criterias.FirstOrDefault(c => c.FieldId == ffContentItem.Field.Id.ToString());
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

        private void showOperators()
        {
            foreach (Control controls in listFieldsContainer.Controls)
            {
                FormField ffContentItem = (FormField)controls.FindControl("ffContent");
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");
                dropdownOperators.Items.Clear();
                List<Operators> OperatorsGets = OperatorsHelper.GetOperators(ffContentItem.Field.Type);
                dropdownOperators.Items.Add(new ListItem(NONE_OPERATOR_VALUE));
                foreach (Operators OperatorGet in OperatorsGets)
                {
                    dropdownOperators.Items.Add(new ListItem(Constants.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
                }
            }
        }

        private void showPeoplePickerUI(PeopleEditor peControl, PermissionAssigment permission)
        {
            foreach (string strMember in permission.Members)
            {
                PickerEntity peMember = new PickerEntity();
                string[] arrPeMember = strMember.Split('#');
                peMember.Key = arrPeMember[1];
                peMember = peControl.ValidateEntity(peMember);
                peControl.Entities.Add(peMember);
            }
        }

        private void showAvailableContentTypes()
        {
            List<SPContentType> contentTypes = this.CurrentList.ContentTypes
                                                    .Cast<SPContentType>()
                                                    .ToList();

            contentTypes.Sort(delegate(SPContentType type1, SPContentType type2)
            {
                return type1.Name.CompareTo(type2.Name);
            });

            drpContentTypes.DataSource = contentTypes;
            drpContentTypes.DataValueField = "Id";
            drpContentTypes.DataTextField = "Name";
            drpContentTypes.DataBind();
            drpContentTypes.Items.Insert(0, new ListItem
            {
                Text = "All Content Types",
                Value = string.Empty
            });
            drpContentTypes.Attributes.Add("onchange", "javascript:ChangeContentTypeS(\"" + drpContentTypes.ClientID + "\");return false;");
        }

        private void showSelectContentType()
        {
            if (this.CurrentContentType != null)
            {
                ltrDescription.Text = CurrentContentType.Description;
                drpContentTypes.SelectedValue = CurrentContentType.Id.ToString();
                documentTypesRow.Visible = !CurrentContentType.Id.IsChildOf(SPBuiltInContentTypeId.Folder);
            }
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

        private Rule getCurrentRule()
        {
            _ruleId = Request.QueryString["RuleId"];

            SecuritySettings setting = this.CurrentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
            if (setting == null) return null;
            Rule ruleGet = setting.Rules.FirstOrDefault(r => r.ID == _ruleId);

            return ruleGet;
        }

        public void RemoveRequiredPropertiesAndExcludeFields()
        {
            foreach (SPField field in this.listFieldsContainer.Fields)
            {
                field.Required = false;
                if (!(field.Type == SPFieldType.Invalid && string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0))
                {
                    field.DefaultValue = null;
                }
                
                // exclude fiedls
                switch (field.Type)
                {
                    case SPFieldType.File:
                    case SPFieldType.URL:
                    case SPFieldType.Attachments:
                    case SPFieldType.AllDayEvent:
                        listFieldsContainer.ExcludeFields += field.Title + ";#";
                        break;

                    case SPFieldType.Note:
                        SPFieldMultiLineText textField = (SPFieldMultiLineText)field;
                        if (textField.RichText == true)
                        {
                            listFieldsContainer.ExcludeFields += field.Title + ";#";
                        }
                        break;
                    //case SPFieldType.Invalid:
                    //    if (string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                    //    {
                    //        LookupFieldWithPicker fieldLookupWithPicker = (LookupFieldWithPicker)field;
                    //        fieldLookupWithPicker.CustomDefaultValue = string.Empty;
                    //    }
                    //    break;
                }
            }
        }

        private void updateRule()
        {
            SecuritySettings setting = this.CurrentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
            Rule rule = setting.Rules.FirstOrDefault(r => r.ID == _ruleId);

            getRuleObject(rule);

            this.CurrentList.SetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp,setting);
        }

        private void getPermissionAssignments(List<PermissionAssigment> permissionAsssigs)
        {
            //set permission assignments
            foreach (Control control in this.repeaterPermissionAssignments.Controls)
            {
                InputFormSection inputFormSection = control.FindControl("inputFormSection") as InputFormSection;

                InputFormControl inputForm = inputFormSection.FindControl("inputForm") as InputFormControl;

                PeopleEditor userEditor = (PeopleEditor)inputForm.FindControl("userEditor");
                HiddenField txtPermissionName = (HiddenField)inputForm.FindControl("txtPermissionName");
                ListBox lsbSelectedFields = (ListBox)inputForm.FindControl("lsbSelectedFields");


                PermissionAssigment perssmissionGet = GetPermissionAssignTo(userEditor, txtPermissionName.Value , lsbSelectedFields);
                if (perssmissionGet != null)
                    permissionAsssigs.Add(perssmissionGet);

            }
        }

        private void getCriterias(List<Criteria> criterias)
        {
            //set criterias
            foreach (Control controls in listFieldsContainer.Controls)
            {
                DropDownList dropdownOperators = (DropDownList)controls.FindControl("dropdownOperators");
                if (dropdownOperators.SelectedIndex != -1 && dropdownOperators.SelectedValue != NONE_OPERATOR_VALUE)
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
        }

        private void addNewRule()
        {
            SecuritySettings setting = CurrentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
            if (setting == null)
                setting = new SecuritySettings();

            Rule rule = new Rule();
            rule.ID = Guid.NewGuid().ToString();

            getRuleObject(rule);

            rule.Order = setting.Rules.Count + 1;
            setting.Rules.Add(rule);

            this.CurrentList.SetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp,setting);
            this.CurrentList.EnsureEventReciever(typeof(SecurityEventHandler), SPEventReceiverType.ItemAdded, SPEventReceiverType.ItemUpdated);
        }

        private void getRuleObject(Rule rule)
        {

            List<Criteria> criterias = new List<Criteria>();
            List<PermissionAssigment> permissionAsssigs = new List<PermissionAssigment>();

            getCriterias(criterias);

            getPermissionAssignments(permissionAsssigs);

            rule.CriteriaList = criterias;
            rule.PermissionAssignments = permissionAsssigs;

            rule.RunOnAdded = chkItemAdded.Checked;
            rule.RunOnFirstUpdate = chkFirstUpdate.Checked;
            rule.RunOnAnyUpdate = chkAnyUpdate.Checked;

            rule.PreserveExistingSecurity = chkPreserveExistingSecurity.Checked;

            rule.ContentTypeId = drpContentTypes.SelectedValue;
            rule.DocumentTypes = txtDocumentTypes.Text.Replace(" ", string.Empty).Trim();
            rule.Name = txtRuleName.Text.Trim();
            if (cboOwnerPermission.SelectedIndex != -1 && cboOwnerPermission.SelectedValue != NONE_VALUE)
                rule.OwnerPermission = cboOwnerPermission.SelectedValue;
            else
                rule.OwnerPermission = string.Empty;
            return;
        }

        private PermissionAssigment GetPermissionAssignTo(PeopleEditor pe, string strPermissionName, ListBox listFields)
        {
            PermissionAssigment permission = new PermissionAssigment();
            permission.PermissionLevel = strPermissionName;

            List<string> fields = new List<string>();
            foreach (ListItem item in listFields.Items)
                fields.Add(item.Value);

            if (fields.Count > 0)
                permission.FieldIds = fields;

            if (pe.ResolvedEntities.Count > 0)
            {
                List<string> members = new List<string>();
                foreach (PickerEntity entity in pe.ResolvedEntities)
                {
                    if (entity.EntityData[PeopleEditorEntityDataKeys.PrincipalType].ToString() == "User")
                        members.Add(entity.EntityData["SPUserID"] + ";#" + entity.Key);
                    else
                        members.Add(entity.EntityData["SPGroupID"] + ";#" + entity.Key);
                }
                permission.Members = members;
            }

            if (permission.Members != null || permission.FieldIds != null)
                return permission;

            return null;
        }

        private List<Rule> reOrderRuleList(List<Rule> rules)
        {
            rules.Sort(delegate(Rule r1, Rule r2) { return r1.Order.CompareTo(r2.Order); });
            for (int i = 1; i <= rules.Count; i++)
            {
                rules[i - 1].Order = i;
            }
            return rules;
        }

        private void showOwnerPermDropDown(string currentPermission)
        {
            SPWeb web = SPContext.Current.Web;
            cboOwnerPermission.Items.Clear();
            var queryRoleDefinitions = from SPRoleDefinition role in web.RoleDefinitions
                                       where role.Hidden == false
                                       select role;

            cboOwnerPermission.DataSource = queryRoleDefinitions;
            cboOwnerPermission.DataTextField = "Name";
            cboOwnerPermission.DataBind();
            cboOwnerPermission.Items.Insert(0, NONE_VALUE);
            cboOwnerPermission.SelectedValue = currentPermission;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Control repeaterItem = ((Button)sender).Parent;
            ListBox lsbAvailabaleFields = (ListBox)repeaterItem.FindControl("lsbAvailabaleFields");
            ListBox lsbSelectedFields = (ListBox)repeaterItem.FindControl("lsbSelectedFields");
            moveSelectedItem(lsbAvailabaleFields, lsbSelectedFields);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Control repeaterItem = ((Button)sender).Parent;
            ListBox lsbAvailabaleFields = (ListBox)repeaterItem.FindControl("lsbAvailabaleFields");
            ListBox lsbSelectedFields = (ListBox)repeaterItem.FindControl("lsbSelectedFields");
            moveSelectedItem(lsbSelectedFields, lsbAvailabaleFields);
        }

        private void moveSelectedItem(ListBox source, ListBox destination)
        {
            List<ListItem> sourceItems = source.Items.Cast<ListItem>().ToList();
            List<ListItem> destinationItems = destination.Items.Cast<ListItem>().ToList();

            foreach (ListItem item in sourceItems)
            {
                if (item.Selected)
                {
                    destinationItems.Add(item);
                    source.Items.Remove(item);
                }
            }
            destinationItems.Sort(delegate(ListItem p1, ListItem p2)
            {
                return p1.Text.CompareTo(p2.Text);
            });

            destination.Items.Clear();
            destination.Items.AddRange(destinationItems.ToArray());
        }
        #endregion

    }
}

