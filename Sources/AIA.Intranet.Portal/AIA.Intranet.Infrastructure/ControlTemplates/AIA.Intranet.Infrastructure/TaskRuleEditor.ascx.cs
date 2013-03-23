using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model.Search;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class TaskRuleEditor : UserControl
    {
        public delegate void ExtenalHandler();
        public event ExtenalHandler OnAfterSaveSuccess;

        #region Properties
        public List<Criteria> CriteriaList
        {
            get
            {
                if (_criteriaList == null)
                    _criteriaList = getCriteriaList();
                return _criteriaList;
            }
            set
            {
                _criteriaList = value;
            }
        }

        public List<SPField> Fields { get; set; }
        public bool FormReadOnly { get; set; }
        #endregion

        #region Fields
        private List<Criteria> _criteriaList = null;
        private List<SPField> _fieldsList = new List<SPField>();
        private List<Control> _fieldsControlList = new List<Control>();
        private List<DropDownList> _operatorsDropdownList = new List<DropDownList>();
        #endregion

        #region Handlers
        protected override void CreateChildControls()
        {
            if (Fields == null)
                return;

            buildForm();

            if (!Page.IsPostBack)
                showCriteria();

            if (FormReadOnly)
                lockAllControl();

            base.CreateChildControls();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.Page.Validate();

            if (this.Page.IsValid)
            {
                CriteriaList = getCriteriaList();
                if (OnAfterSaveSuccess != null)
                {
                    OnAfterSaveSuccess();
                    //Close popup
                    //Context.Response.Clear();
                    Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    Context.Response.Flush();
                    Context.Response.End();
                    return;
                }
            }
        }
        #endregion

        #region Methods
        private void showCriteria()
        {
            if (CriteriaList == null || CriteriaList.Count == 0) return;
            if (_fieldsList.Count == 0) return;
            if (_fieldsControlList.Count == 0) return;

            for (int i = 0; i < _fieldsList.Count; i++)
            {
                SPField field = _fieldsList[i];

                if (!CriteriaList.Exists(s => s.FieldId == field.Id.ToString())) continue;

                Criteria criteria = CriteriaList.FirstOrDefault(s => s.FieldId == field.Id.ToString());
                List<Criteria> criterias = CriteriaList.FindAll(s => s.FieldId == field.Id.ToString());

                _operatorsDropdownList[i].SelectedValue = criteria.Operator.ToString();
                _operatorsDropdownList[i].Style.Add("width", "120px");
                Operators enumOperatorSelected = (Operators)Enum.Parse(typeof(Operators), _operatorsDropdownList[i].SelectedValue);
                switch (field.Type)
                {
                    case SPFieldType.Boolean:
                        CheckBox chkField = (CheckBox)_fieldsControlList[i];
                        chkField.Checked = Convert.ToBoolean(Convert.ToByte(criteria.Value));
                        break;

                    case SPFieldType.Currency:
                    case SPFieldType.Integer:
                    case SPFieldType.Number:
                    case SPFieldType.Calculated:
                    case SPFieldType.Computed:
                    case SPFieldType.Note:
                    case SPFieldType.Text:
                    case SPFieldType.File:
                        TextBox txtInput = (TextBox)_fieldsControlList[i];
                        txtInput.Text = criteria.Value;
                        break;

                    case SPFieldType.Choice:
                        DropDownList ddlValueChoice = (DropDownList)_fieldsControlList[i];
                        ddlValueChoice.SelectedValue = criteria.Value;
                        break;

                    case SPFieldType.DateTime:
                        SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                        DateTimeControl dtcValueDate = (DateTimeControl)_fieldsControlList[i];
                        dtcValueDate.SelectedDate = Convert.ToDateTime(criteria.Value).ToUniversalTime();
                        break;

                    case SPFieldType.Lookup:
                        if (string.IsNullOrEmpty(criteria.Value)) continue;

                        SPFieldLookup fieldLookup = (SPFieldLookup)field;
                        if (!fieldLookup.AllowMultipleValues)
                        {
                            DropDownList ddlValueLookup = (DropDownList)_fieldsControlList[i];
                            SPFieldLookupValue valueLookup = new SPFieldLookupValue(criteria.Value);
                            ddlValueLookup.SelectedValue = valueLookup.LookupId.ToString();
                        }
                        else
                        {
                            CheckBoxList chkValueMultiLookup = (CheckBoxList)_fieldsControlList[i];
                            SPFieldLookupValueCollection multiLookupValues = new SPFieldLookupValueCollection(criteria.Value);
                            foreach (SPFieldLookupValue multiLookupValue in multiLookupValues)
                                foreach (ListItem item in chkValueMultiLookup.Items)
                                    if (item.Value == multiLookupValue.LookupId.ToString())
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                        }
                        break;

                    case SPFieldType.MultiChoice:
                        if (string.IsNullOrEmpty(criteria.Value)) continue;
                        CheckBoxList chkValueMultiChoice = (CheckBoxList)_fieldsControlList[i];
                        SPFieldMultiChoiceValue multiChoiceValues = new SPFieldMultiChoiceValue(criteria.Value);
                        for (int iChoice = 0; iChoice < multiChoiceValues.Count; iChoice++)
                        {
                            string multiChoiceValue = multiChoiceValues[iChoice];
                            foreach (ListItem item in chkValueMultiChoice.Items)
                                if (item.Text == multiChoiceValue)
                                {
                                    item.Selected = true;
                                    break;
                                }
                        }
                        break;

                    case SPFieldType.User:
                        if (string.IsNullOrEmpty(criteria.Value)) continue;
                        PeopleEditor peoValue = (PeopleEditor)_fieldsControlList[i];
                        SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(SPContext.Current.Web, criteria.Value);

                        foreach (SPFieldUserValue userValue in userValues)
                            showPeoplePickerUI(peoValue, userValue.LookupValue);
                        break;
                }
            }
        }

        private void showPeoplePickerUI(PeopleEditor peControl, string strLoginName)
        {
            PickerEntity peMember = new PickerEntity();
            peMember.Key = strLoginName;
            peMember = peControl.ValidateEntity(peMember);
            peControl.Entities.Add(peMember);
        }

        private void buildForm()
        {
            foreach (SPField field in Fields)
            {
                if (ignoreField(field)) continue;
                DropDownList ddlOperators = buildCriteriaOperatorDropDown(field);

                Panel pnlField = new Panel();
                Control control = buildValueSelectorControl(field);

                if (control != null)
                {
                    _fieldsControlList.Add(control);
                    pnlField.Controls.Add(control.Parent ?? control);
                    addNewCriteriaRow(field.Title, ddlOperators, pnlField);
                }
                _fieldsList.Add(field);
            }
        }

        private void lockAllControl()
        {
            btnSave.Visible = false;
            btnCancel.Visible = true;
            foreach (Control control in this._fieldsControlList)
            {
                lockControl(control);
            }
            foreach (DropDownList control in _operatorsDropdownList)
            {
                lockControl(control);
            }
        }

        private void lockControl(Control control)
        {
            if (control is WebControl)
            {
                WebControl webControl = (WebControl)control;
                webControl.Enabled = false;
            }
            if (control is DateTimeControl)
            {
                DateTimeControl dateTimeControl = (DateTimeControl)control;
                dateTimeControl.Enabled = false;
            }
        }
        private bool ignoreField(SPField field)
        {
            bool isIgnore = true;
            if ((field.Hidden && field.StaticName != Constants.LINK_TO_FILE_COLUMN)
                    || (field.ReadOnlyField && field.StaticName != Constants.LINK_TO_FILE_COLUMN))
                return true;
            switch (field.Type)
            {
                case SPFieldType.AllDayEvent:
                case SPFieldType.Attachments:
                case SPFieldType.CrossProjectLink:
                case SPFieldType.Error:
                case SPFieldType.GridChoice:
                case SPFieldType.Invalid:
                case SPFieldType.ModStat:
                case SPFieldType.PageSeparator:
                case SPFieldType.Recurrence:
                case SPFieldType.ThreadIndex:
                case SPFieldType.Threading:
                case SPFieldType.URL:
                case SPFieldType.WorkflowEventType:
                case SPFieldType.WorkflowStatus:
                    isIgnore = true;
                    break;

                case SPFieldType.File:
                    if (field.StaticName == Constants.LINK_TO_FILE_COLUMN)
                        isIgnore = false;
                    break;

                case SPFieldType.Boolean:
                case SPFieldType.Calculated:
                case SPFieldType.Choice:
                case SPFieldType.Computed:
                case SPFieldType.Currency:
                case SPFieldType.DateTime:
                case SPFieldType.Integer:
                case SPFieldType.Lookup:
                case SPFieldType.MultiChoice:
                case SPFieldType.Note:
                case SPFieldType.Number:
                case SPFieldType.Text:
                case SPFieldType.User:
                    isIgnore = false;
                    break;
            }
            return isIgnore;
        }

        private void addNewCriteriaRow(string titleField, DropDownList operatorDropdown, Panel fieldValuePanel)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.Attributes.Add("class", "ms-formlabel");
            cell2.VAlign = "top";
            cell2.Controls.Add(new LiteralControl(titleField));
            row.Cells.Add(cell2);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.Attributes.Add("class", "ms-formlabel");
            cell3.VAlign = "top";
            cell3.Controls.Add(operatorDropdown);
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.Attributes.Add("class", "ms-formbody");
            cell4.VAlign = "top";
            cell4.Controls.Add(fieldValuePanel);
            row.Cells.Add(cell4);
            tblFieldsValueDefinition.Rows.Add(row);
        }

        private DropDownList buildCriteriaOperatorDropDown(SPField field)
        {
            DropDownList ddlOperators = new DropDownList();
            ControlHelper.LoadOperatorDropdown(field, ddlOperators);
            _operatorsDropdownList.Add(ddlOperators);
            ddlOperators.Style.Add(HtmlTextWriterStyle.Width, "100px");
            return ddlOperators;
        }

        private Control buildValueSelectorControl(SPField field)
        {
            Control control = null;
            switch (field.Type)
            {
                case SPFieldType.Boolean:
                    control = new CheckBox();
                    break;

                case SPFieldType.File:
                case SPFieldType.Calculated:
                case SPFieldType.Computed:
                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Note:
                case SPFieldType.Number:
                case SPFieldType.Text:
                    control = new TextBox() { CssClass = "ms-long" };
                    break;

                case SPFieldType.Choice:
                    SPFieldChoice fieldChoice = (SPFieldChoice)field;
                    DropDownList ddlValueChoice = new DropDownList();

                    foreach (string value in fieldChoice.Choices)
                    {
                        ddlValueChoice.Items.Add(new ListItem(value));
                    }

                    ddlValueChoice.Items.Insert(0, string.Empty);
                    control = ddlValueChoice;
                    break;

                case SPFieldType.DateTime:
                    SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                    DateTimeControl dtcValueDate = new DateTimeControl();
                    dtcValueDate.DateOnly = (fieldDate.DisplayFormat == SPDateTimeFieldFormatType.DateOnly);
                    control = dtcValueDate;
                    break;

                case SPFieldType.Lookup:
                    SPFieldLookup fieldLookup = (SPFieldLookup)field;
                    Dictionary<string, string> lookupValues = getLookupValues(fieldLookup);
                    if (fieldLookup.AllowMultipleValues == false)
                    {
                        DropDownList ddlValueLookup = new DropDownList();
                        foreach (KeyValuePair<string, string> k in lookupValues)
                        {
                            ddlValueLookup.Items.Add(new ListItem(k.Value, k.Key));
                        }

                        ddlValueLookup.Items.Insert(0, string.Empty);
                        control = ddlValueLookup;
                    }
                    else
                    {
                        CheckBoxList chkValueMultiLookup = new CheckBoxList();
                        foreach (KeyValuePair<string, string> k in lookupValues)
                            chkValueMultiLookup.Items.Add(new ListItem(k.Value, k.Key));
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("style", "overflow: auto;height:100px;width:100%;");
                        div.Controls.Add(chkValueMultiLookup);

                        control = chkValueMultiLookup;
                    }
                    break;

                case SPFieldType.MultiChoice:
                    SPFieldMultiChoice fieldMultichoice = (SPFieldMultiChoice)field;
                    CheckBoxList chkValueMultiChoice = new CheckBoxList();
                    foreach (string value in fieldMultichoice.Choices)
                        chkValueMultiChoice.Items.Add(new ListItem(value));
                    control = chkValueMultiChoice;

                    HtmlGenericControl div2 = new HtmlGenericControl("div");
                    div2.Attributes.Add("style", "overflow: auto;height:100px;width:100%;");
                    div2.Controls.Add(chkValueMultiChoice);

                    control = chkValueMultiChoice;
                    break;

                case SPFieldType.User:
                    SPFieldUser fieldUser = (SPFieldUser)field;
                    PeopleEditor peoValue = new PeopleEditor();
                    peoValue.MultiSelect = fieldUser.AllowMultipleValues;
                    peoValue.SelectionSet = (fieldUser.SelectionMode == SPFieldUserSelectionMode.PeopleOnly) ? "User" : "User,SPGroup ";
                    control = peoValue;
                    break;
            }
            return control;
        }

        private Dictionary<string, string> getLookupValues(SPFieldLookup fieldLookup)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            using (SPWeb web = SPContext.Current.Site.OpenWeb(fieldLookup.LookupWebId))
            {
                SPList list = web.Lists[new Guid(fieldLookup.LookupList)];
                foreach (SPListItem item in list.Items)
                    ret.Add(item.ID.ToString(), item[fieldLookup.LookupField].ToString());
            }
            return ret;
        }

        private Criteria newCriteria(SPField field, Operators enumOperatorSelected)
        {
            Criteria criteria = new Criteria();
            criteria.FieldId = field.Id.ToString();
            criteria.Operator = enumOperatorSelected;
            return criteria;
        }

        private List<Criteria> getCriteriaList()
        {
            List<Criteria> criteriaList = new List<Criteria>();

            if (Fields == null) return criteriaList;
            if (_fieldsList.Count == 0) return criteriaList;
            if (_fieldsControlList.Count == 0) return criteriaList;

            for (int i = 0; i < _fieldsList.Count; i++)
            {
                if (_operatorsDropdownList[i].SelectedValue == Constants.NOT_APPLY_VALUE) continue;

                SPField field = _fieldsList[i];
                Operators enumOperatorSelected = (Operators)Enum.Parse(typeof(Operators), _operatorsDropdownList[i].SelectedValue);

                Criteria criteria = newCriteria(field, enumOperatorSelected);
                switch (field.Type)
                {
                    case SPFieldType.Boolean:
                        CheckBox chkField = (CheckBox)_fieldsControlList[i];
                        criteria.Value = chkField.Checked ? "1" : "0";
                        break;

                    case SPFieldType.Currency:
                        TextBox txtCurrency = (TextBox)_fieldsControlList[i];
                        if (string.IsNullOrEmpty(txtCurrency.Text)) continue;
                        criteria.Value = txtCurrency.Text;
                        break;

                    case SPFieldType.Integer:
                    case SPFieldType.Number:
                        TextBox txtNumber = (TextBox)_fieldsControlList[i];
                        if (string.IsNullOrEmpty(txtNumber.Text)) continue;
                        criteria.Value = txtNumber.Text;
                        break;

                    case SPFieldType.Calculated:
                    case SPFieldType.Computed:
                    case SPFieldType.Note:
                    case SPFieldType.Text:
                    case SPFieldType.File:
                        TextBox txtText = (TextBox)_fieldsControlList[i];
                        criteria.Value = txtText.Text;
                        break;

                    case SPFieldType.Choice:
                        DropDownList ddlValueChoice = (DropDownList)_fieldsControlList[i];
                        criteria.Value = ddlValueChoice.SelectedValue;
                        break;

                    case SPFieldType.DateTime:
                        SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                        DateTimeControl dtcValueDate = (DateTimeControl)_fieldsControlList[i];
                        criteria.Value = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtcValueDate.SelectedDate);
                        break;

                    case SPFieldType.Lookup:
                        SPFieldLookup fieldLookup = (SPFieldLookup)field;

                        if (fieldLookup.AllowMultipleValues == false)
                        {
                            DropDownList ddlValueLookup = (DropDownList)_fieldsControlList[i];
                            if (string.IsNullOrEmpty(ddlValueLookup.SelectedItem.Value)) continue;
                            criteria.Value = ddlValueLookup.SelectedItem.Value + ";#" + ddlValueLookup.SelectedItem.Text;
                        }
                        else
                        {
                            CheckBoxList chkValueMultiLookup = (CheckBoxList)_fieldsControlList[i];
                            string multiLookupValue = string.Empty;
                            foreach (ListItem item in chkValueMultiLookup.Items)
                                if (item.Selected)
                                    multiLookupValue += item.Value + ";#" + item.Text + ";#";

                            if (!string.IsNullOrEmpty(multiLookupValue))
                            {
                                multiLookupValue = multiLookupValue.Remove(multiLookupValue.LastIndexOf(";#"), 2);
                                criteria.Value = multiLookupValue;
                            }
                        }
                        break;

                    case SPFieldType.MultiChoice:
                        CheckBoxList chkValueMultiChoice = (CheckBoxList)_fieldsControlList[i];
                        string multiChoiceValue = string.Empty;
                        foreach (ListItem item in chkValueMultiChoice.Items)
                            if (item.Selected)
                                multiChoiceValue += item.Value + ";#";


                        if (!string.IsNullOrEmpty(multiChoiceValue))
                        {
                            multiChoiceValue = multiChoiceValue.Remove(multiChoiceValue.LastIndexOf(";#"), 2);
                            criteria.Value = multiChoiceValue;
                        }
                        break;

                    case SPFieldType.User:
                        PeopleEditor peoValue = (PeopleEditor)_fieldsControlList[i];
                        if (peoValue.ResolvedEntities.Count == 0) continue;

                        string userValue = string.Empty;
                        foreach (PickerEntity entity in peoValue.ResolvedEntities)
                            if (entity.EntityData[PeopleEditorEntityDataKeys.PrincipalType].ToString() == "User")
                                userValue += entity.EntityData["SPUserID"] + ";#" + entity.Key + ";#";
                            else
                                userValue += entity.EntityData["SPGroupID"] + ";#" + entity.Key + ";#";

                        userValue = userValue.Remove(userValue.LastIndexOf(";#"), 2);
                        criteria.Value = userValue;
                        break;
                }
                criteriaList.Add(criteria);
            }

            return criteriaList;
        }
        #endregion
    }
}
