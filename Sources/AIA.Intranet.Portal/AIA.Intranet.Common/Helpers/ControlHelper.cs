using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Search.Settings;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Helpers
{
    public class ControlHelper
    {
        private const string GENERIC_CONTROL_ID = "GenericFieldControl";
        private const string SEPARATOR = "|";
        //public static void LoadOperatorDropdown(SPField field, DropDownList ddlOperator)
        //{
        //    List<Operators> OperatorsGets = OperatorsHelper.GetOperators(field.Type);

        //    ddlOperator.Items.Add(new ListItem(Constants.NOT_APPLY_VALUE));

        //    foreach (Operators OperatorGet in OperatorsGets)
        //    {
        //        ddlOperator.Items.Add(new ListItem(OperatorsHelper.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
        //    }
        //}
        
        public static void LoadOperatorDisplayType(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(OperatorDisplayType.Select.ToString()));
            ddl.Items.Add(new ListItem(OperatorDisplayType.Display.ToString()));
            ddl.Items.Add(new ListItem(OperatorDisplayType.Hide.ToString()));
        }

        public static void LoadFieldsToDropdown(DropDownList ddlTemp, List<SPField> fields)
        {
            LoadFieldsToDropdown(ddlTemp, fields, true);
        }

        public static void LoadFieldsToDropdown(DropDownList ddlTemp, List<SPField> fields, bool addSelectRow)
        {
            ddlTemp.Items.Clear();
            if (addSelectRow && fields.Count > 1)
                ddlTemp.Items.Add(new ListItem(Constants.CHOOSE_PROPERTY, string.Empty));

            List<SPField> listSPFields = new List<SPField>();
            foreach (SPField field in fields)
            {
                if (field.Hidden && field.StaticName != Constants.LINK_TO_FILE_COLUMN) continue;
                switch (field.Type)
                {
                    case SPFieldType.AllDayEvent:
                    case SPFieldType.Attachments:
                    case SPFieldType.CrossProjectLink:
                    case SPFieldType.Error:

                    case SPFieldType.GridChoice:

                    case SPFieldType.ModStat:
                    case SPFieldType.PageSeparator:
                    case SPFieldType.Recurrence:
                    case SPFieldType.ThreadIndex:
                    case SPFieldType.Threading:
                    //case SPFieldType.URL:
                    case SPFieldType.WorkflowEventType:
                    case SPFieldType.WorkflowStatus:
                        continue;

                    case SPFieldType.URL:
                        SPFieldUrl fieldUrl = (SPFieldUrl)field;
                        if (fieldUrl.DisplayFormat == SPUrlFieldFormatType.Image)
                            listSPFields.Add(field);
                        break;

                    case SPFieldType.Invalid:
                        if (string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                        {
                            listSPFields.Add(field);
                        }
                        break;

                    case SPFieldType.File:
                        if (field.StaticName == Constants.LINK_TO_FILE_COLUMN)
                        {
                            listSPFields.Add(field);
                        }
                        break;

                    case SPFieldType.Boolean:
                    case SPFieldType.Calculated:
                    case SPFieldType.Choice:
                    case SPFieldType.Computed:
                    case SPFieldType.Currency:
                    case SPFieldType.DateTime:
                    case SPFieldType.Integer:
                    
                    case SPFieldType.MultiChoice:
                    case SPFieldType.Note:
                    case SPFieldType.Number:
                    case SPFieldType.Text:
                    case SPFieldType.User:
                        listSPFields.Add(field);
                        break;

                    case SPFieldType.Lookup:
                        SPFieldLookup lookup = field as SPFieldLookup;
                        if (!string.IsNullOrEmpty(lookup.LookupList) && string.Compare("lookup.LookupList", "self", true) != 0)
                        {
                            listSPFields.Add(field);
                        }
                        break;
                }
            }
            listSPFields.Sort(delegate(SPField f1, SPField f2) { return f1.Title.CompareTo(f2.Title); });
            foreach (SPField field in listSPFields)
            {
                
                if (listSPFields.Count > 1)
                {
                    if (listSPFields.FindAll(f => f.Title == field.Title).Count > 1
                        && field.Title != field.StaticName)
                        ddlTemp.Items.Add(new ListItem(field.Title + " (" + ((field.StaticName == Constants.LINK_TO_FILE_COLUMN) ? "Display Name" : field.StaticName) + ")", field.Id.ToString()));
                    else
                        ddlTemp.Items.Add(new ListItem(field.Title, field.Id.ToString()));
                }
                else
                {
                    if (field.Title != field.StaticName)
                        ddlTemp.Items.Add(new ListItem(field.Title + " (" + ((field.StaticName == Constants.LINK_TO_FILE_COLUMN) ? "Display Name" : field.StaticName) + ")", field.Id.ToString()));
                    else
                        ddlTemp.Items.Add(new ListItem(field.Title, field.Id.ToString()));
                }
            }
        }

        public static void LoadOperatorDropdownNotDefault(SPField field, DropDownList ddlOperator, bool useKeyWord)
        {
            ddlOperator.Items.Clear();
            List<Operators> OperatorsGets = null;
            if (field == null)
            {
                OperatorsGets = new List<Operators>() { Operators.Equal, Operators.NotEqual, Operators.IsNull };
            }
            else
            {
                if (SPFieldType.Invalid == field.Type && useKeyWord &&
                     string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                {
                    ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.Equal], Operators.Equal.ToString()));
                    ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.NotEqual], Operators.NotEqual.ToString()));
                    ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.Contains], Operators.Contains.ToString()));
                    ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.StartsWith], Operators.StartsWith.ToString()));
                    ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.IsNull], Operators.IsNull.ToString()));
                    return;
                }
                OperatorsGets = OperatorsHelper.GetOperators(field);
            }

            foreach (Operators OperatorGet in OperatorsGets)
            {
                ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
            }
        }
       
        public static void LoadOperatorListBox(SPField field, ListBox listbox, bool useKeyWord)
        {
            listbox.Items.Clear();

            List<Operators> OperatorsGets = null;
            if (field == null)
            {
                OperatorsGets = new List<Operators>() { Operators.Equal, Operators.NotEqual, Operators.IsNull };
            }
            else
            {
                if (SPFieldType.Invalid == field.Type && useKeyWord &&
                       string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                {
                    listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.Equal], Operators.Equal.ToString()));
                    listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.NotEqual], Operators.NotEqual.ToString()));
                    listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.Contains], Operators.Contains.ToString()));
                    listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.StartsWith], Operators.StartsWith.ToString()));
                    listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[Operators.IsNull], Operators.IsNull.ToString()));
                    return;
                }
                OperatorsGets = OperatorsHelper.GetOperators(field);
            }

            foreach (Operators OperatorGet in OperatorsGets)
            {
                listbox.Items.Add(new ListItem(Constants.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
            }
        }

        public static void LoadFieldsToDropdown(DropDownList ddlTemp, SPFieldCollection fieldCollection)
        {
            LoadFieldsToDropdown(ddlTemp, fieldCollection, true);
        }

        public static void LoadFieldsToDropdown(DropDownList ddlTemp, SPFieldCollection fieldCollection, bool addSelectRow)
        {
            List<SPField> fields = fieldCollection.Cast<SPField>().ToList();
            LoadFieldsToDropdown(ddlTemp, fields, addSelectRow);
        }


        public static void LoadOrderDropdown(DropDownList ddlTemp)
        {
            LoadOrderDropdown(ddlTemp, Constants.MAX_NUMBER_OF_FIELDS);
        }

        public static void LoadOrderDropdown(DropDownList ddlTemp, int maxNumber)
        {
            for (int i = 1; i <= maxNumber; i++)
                ddlTemp.Items.Add(new ListItem(i.ToString()));
        }

        public static void LoadOperatorDropdown(SPField field, DropDownList ddlOperator)
        {
            List<Operators> OperatorsGets = OperatorsHelper.GetOperators(field);

            ddlOperator.Items.Add(new ListItem(Constants.NOT_APPLY_VALUE));

            foreach (Operators OperatorGet in OperatorsGets)
            {

                ddlOperator.Items.Add(new ListItem(Constants.OperatorDisplayNames[OperatorGet], OperatorGet.ToString()));
            }
        }

        public static void LoadSortDropdown(DropDownList ddlSort)
        {
            ddlSort.Items.Add(new ListItem(Constants.NOT_APPLY_VALUE, SortType.NotApply.ToString()));
            ddlSort.Items.Add(new ListItem(SortType.Ascending.ToString()));
            ddlSort.Items.Add(new ListItem(SortType.Descending.ToString()));
        }

        public static void LoadScopeDropdown(DropDownList dropScope)
        {
            dropScope.Items.Clear();
            dropScope.Items.Add(new ListItem(Constants.SearchScopeDisplayNames[SearchScope.CurrentSite], SearchScope.CurrentSite.ToString()));
            dropScope.Items.Add(new ListItem(Constants.SearchScopeDisplayNames[SearchScope.CurrentAndSubSite], SearchScope.CurrentAndSubSite.ToString()));
            dropScope.Items.Add(new ListItem(Constants.SearchScopeDisplayNames[SearchScope.SiteCollection], SearchScope.SiteCollection.ToString()));
        }

        public static void LoadListBaseTypeDropdown(DropDownList dropListBaseType)
        {
            dropListBaseType.Items.Clear();
            dropListBaseType.Items.Add(new ListItem(Constants.SearchListBaseTypeDisplayNames[SearchListBaseType.GenericList]
                , SearchListBaseType.GenericList.ToString()));
            dropListBaseType.Items.Add(new ListItem(Constants.SearchListBaseTypeDisplayNames[SearchListBaseType.DocumentLibrary]
                , SearchListBaseType.DocumentLibrary.ToString()));
            dropListBaseType.Items.Add(new ListItem(Constants.SearchListBaseTypeDisplayNames[SearchListBaseType.DiscussionForum]
                , SearchListBaseType.DiscussionForum.ToString()));
            dropListBaseType.Items.Add(new ListItem(Constants.SearchListBaseTypeDisplayNames[SearchListBaseType.VoteOrSurvey]
                , SearchListBaseType.VoteOrSurvey.ToString()));
            dropListBaseType.Items.Add(new ListItem(Constants.SearchListBaseTypeDisplayNames[SearchListBaseType.IssuesList]
                , SearchListBaseType.IssuesList.ToString()));
        }

        //public static List<FieldSetting> AddECBFieldToResultField(SearchResultsEditorSettings Setting)
        //{
        //    List<FieldSetting> ReturnFields = new List<FieldSetting>();
        //    ReturnFields = Setting.ResultFields;

        //    if (string.IsNullOrEmpty(Setting.ECBFieldID) || string.Compare(Setting.ECBFieldID, "Choose property") == 0)
        //        return ReturnFields;

        //    foreach (FieldSetting field in Setting.ResultFields)
        //    {
        //        if (field.FieldId.CompareTo(Setting.ECBFieldID) == 0)
        //            return ReturnFields;
        //    }
        //    FieldSetting ECBField = new FieldSetting() { FieldId = Setting.ECBFieldID, Order = 0, Sort = SortType.NotApply };
        //    ReturnFields.Add(ECBField);
        //    //sort order
        //    ReturnFields.Sort(delegate(FieldSetting f1, FieldSetting f2) { return f1.Order.CompareTo(f2.Order); });

        //    for (int i = 0; i < ReturnFields.Count; i++)
        //    {
        //        FieldSetting field = ReturnFields[i];
        //        field.Order = i + 1;
        //    }
        //    return ReturnFields;
        //}

        public static Control BuildValueSelectorControl(SPField field, string displayValue)
        {
            Control control = null;
            if (field == null) { control = new TextBox() { CssClass = "ms-long", Text = displayValue, Width = 380 }; }
            else
            {

                switch (field.Type)
                {
                    case SPFieldType.Boolean:

                        CheckBox checkbox = new CheckBox() { };
                        if (!string.IsNullOrEmpty(displayValue))
                        {
                            bool value;
                            if (bool.TryParse(displayValue, out value))
                            {
                                checkbox.Checked = value;
                            }

                        }
                        control = checkbox;
                        break;

                    case SPFieldType.File:
                    case SPFieldType.Calculated:
                    case SPFieldType.Computed:
                    case SPFieldType.Currency:
                    case SPFieldType.Integer:
                    case SPFieldType.Note:
                    case SPFieldType.Number:
                    case SPFieldType.Text:
                    case SPFieldType.URL:
                    case SPFieldType.Invalid:
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue, Width = 380 };
                        break;

                    case SPFieldType.Choice:
                        SPFieldChoice fieldChoice = (SPFieldChoice)field;
                        DropDownList ddlValueChoice = new DropDownList();

                        foreach (string value in fieldChoice.Choices)
                        {
                            ddlValueChoice.Items.Add(new ListItem(value));
                        }

                        ddlValueChoice.Items.Insert(0, new ListItem("Select an item",""));
                        ddlValueChoice.SelectedIndex = ddlValueChoice.Items.IndexOf(ddlValueChoice.Items.FindByValue(displayValue));
                        control = ddlValueChoice;
                        break;

                    case SPFieldType.DateTime:
                        SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                        DateTimeControl dtcValueDate = new DateTimeControl();
                        dtcValueDate.DateOnly = (fieldDate.DisplayFormat == SPDateTimeFieldFormatType.DateOnly);
                        if (!string.IsNullOrEmpty(displayValue))
                        {
                            DateTime date;
                            if (DateTime.TryParse(displayValue, out date))
                            {
                                dtcValueDate.SelectedDate = date;
                            }
                        }

                        control = dtcValueDate;
                        break;

                    case SPFieldType.Lookup:
                        SPFieldLookup fieldLookup = (SPFieldLookup)field;
                        control = generateLookupControl(control, fieldLookup, displayValue);

                        break;

                    case SPFieldType.MultiChoice:
                        SPFieldMultiChoice fieldMultichoice = (SPFieldMultiChoice)field;
                        CheckBoxList chkValueMultiChoice = new CheckBoxList();
                        foreach (string value in fieldMultichoice.Choices)
                        {
                            ListItem item = new ListItem(value);
                            if (!string.IsNullOrEmpty(displayValue))
                            {
                                string[] arr = displayValue.Split(SEPARATOR.ToCharArray());
                                item.Selected = arr.Contains(value);
                            }

                            chkValueMultiChoice.Items.Add(item);
                        }
                        control = chkValueMultiChoice;

                        HtmlGenericControl div2 = new HtmlGenericControl("div");
                        div2.Attributes.Add("style", "overflow: auto;height:100px;width:100%;");
                        div2.Controls.Add(chkValueMultiChoice);

                        control = div2;
                        break;

                    case SPFieldType.User:
                        SPFieldUser fieldUser = (SPFieldUser)field;
                        PeopleEditor peoValue = new PeopleEditor();
                        peoValue.MultiSelect = fieldUser.AllowMultipleValues;
                        peoValue.SelectionSet = (fieldUser.SelectionMode == SPFieldUserSelectionMode.PeopleOnly) ? "User" : "User,SPGroup ";
                        if (!string.IsNullOrEmpty(displayValue))
                        {
                            string[] arr = displayValue.Split(SEPARATOR.ToCharArray());

                            ArrayList list = new ArrayList();
                            foreach (string s in arr)
                            {
                                PickerEntity peMember = new PickerEntity();
                                string[] arrPeMember = s.Split('#');
                                peMember.Key = arrPeMember[1];
                                peMember = peoValue.ValidateEntity(peMember);
                                list.Add(peMember);
                            }
                            peoValue.UpdateEntities(list);
                        }
                        control = peoValue;
                        break;
                    default:
                        control = new TextBox() { CssClass = "ms-long", Width = 380, Text = displayValue };
                        break;
                }
            }
            control.ID = GENERIC_CONTROL_ID;
            return control;
        }

        private static Control generateLookupControl(Control control, SPFieldLookup fieldLookup, string displayValue)
        {
            Dictionary<string, string> lookupValues = getLookupValues(fieldLookup);
            if (fieldLookup.AllowMultipleValues == false)
            {
                DropDownList ddlValueLookup = new DropDownList();
                foreach (KeyValuePair<string, string> k in lookupValues)
                {
                    ddlValueLookup.Items.Add(new ListItem(k.Value, k.Key));
                }

                ddlValueLookup.Items.Insert(0, string.Empty);
                if (!string.IsNullOrEmpty(displayValue))
                {
                    string[] arr = displayValue.Split(";#".ToCharArray());

                    SetDropDownListValue(ddlValueLookup, arr[0]);
                }
                control = ddlValueLookup;
            }
            else
            {
                CheckBoxList chkValueMultiLookup = new CheckBoxList();
                string[] arr = null;
                if (!string.IsNullOrEmpty(displayValue))
                {
                    arr = displayValue.Split(SEPARATOR.ToCharArray());
                }

                foreach (KeyValuePair<string, string> k in lookupValues)
                {
                    ListItem item = new ListItem()
                    {
                        Value = k.Key,
                        Text = k.Value,
                        Selected = arr != null ? arr.Contains(k.Key + ";#" + k.Value) : false
                    };
                    
                    chkValueMultiLookup.Items.Add(item);
                }
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("style", "overflow: auto;height:100px;width:100%;");
                div.Controls.Add(chkValueMultiLookup);

                control = div;
            }
            return control;
        }

        private static Dictionary<string, string> getLookupValues(SPFieldLookup fieldLookup)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (string.Compare(fieldLookup.LookupList, "Self") == 0 || string.IsNullOrEmpty(fieldLookup.LookupList))
            {
                return ret;
            }
            else
                using (SPWeb web = SPContext.Current.Site.OpenWeb(fieldLookup.LookupWebId))
                {
                    SPList list = web.Lists[new Guid(fieldLookup.LookupList)];
                    foreach (SPListItem item in list.Items)
                        if (item[fieldLookup.LookupField] != null)
                            ret.Add(item.ID.ToString(), item[fieldLookup.LookupField].ToString());
                }
            return ret;
        }

        protected static void SetDropDownListValue(DropDownList control, string value)
        {
            ListItem item = control.Items.FindByValue(value);
            if (item != null)
                control.SelectedValue = item.Value;
        }

        public static string GetControlValue(Control control, SPField field)
        {
            string result = null;
            if (field == null || control == null) return string.Empty;

            switch (field.Type)
            {
                case SPFieldType.Boolean:
                    result = ((CheckBox)control).Checked.ToString();
                    break;

                case SPFieldType.File:
                case SPFieldType.Calculated:
                case SPFieldType.Computed:
                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Note:
                case SPFieldType.Number:
                case SPFieldType.Text:
                case SPFieldType.URL:
                case SPFieldType.Invalid:
                    result = ((TextBox)control).Text.Trim();
                    break;

                case SPFieldType.Choice:
                    SPFieldChoice fieldChoice = (SPFieldChoice)field;

                    DropDownList ddlValueChoice = ((DropDownList)control);
                    result = ddlValueChoice.SelectedValue;

                    //ddlValueChoice.Items.Insert(0, string.Empty);
                    //control = ddlValueChoice;
                    break;

                case SPFieldType.DateTime:
                    SPFieldDateTime fieldDate = (SPFieldDateTime)field;

                    DateTimeControl dtcValueDate = ((DateTimeControl)control);
                    if (!dtcValueDate.IsDateEmpty)
                        result = dtcValueDate.SelectedDate.ToString();
                    break;

                case SPFieldType.Lookup:
                    SPFieldLookup fieldLookup = (SPFieldLookup)field;
                    result = GetValueFromControlLookup(control, fieldLookup);

                    break;

                case SPFieldType.MultiChoice:
                    SPFieldMultiChoice fieldMultichoice = (SPFieldMultiChoice)field;
                    HtmlGenericControl div2 = (HtmlGenericControl)control;
                    CheckBoxList chkValueMultiChoice = (CheckBoxList)div2.Controls[0];

                    foreach (var item in chkValueMultiChoice.Items.Cast<ListItem>().Where(p => p.Selected))
                        result += item.Value + SEPARATOR;

                    result = result.TrimEnd(SEPARATOR.ToCharArray());
                    break;

                case SPFieldType.User:
                    SPFieldUser fieldUser = (SPFieldUser)field;
                    PeopleEditor peoValue = (PeopleEditor)control;
                    string arr = "";
                    foreach (PickerEntity entity in peoValue.ResolvedEntities)
                    {
                        if (entity.EntityData[PeopleEditorEntityDataKeys.PrincipalType].ToString() == "User")
                        {
                            arr += entity.EntityData["SPUserID"] + ";#" + entity.Key + SEPARATOR;
                        }
                        else
                        {
                            arr += entity.EntityData["SPGroupID"] + ";#" + entity.Key + SEPARATOR;

                        }
                    }
                    result = arr.TrimEnd(SEPARATOR.ToCharArray());
                    break;

                default:
                    TextBox txt = (TextBox)control;
                    result = txt.Text;
                    break;
            }

            return result;

        }

        private static string GetValueFromControlLookup(Control control, SPFieldLookup fieldLookup)
        {
            string results = string.Empty;
            if (!fieldLookup.AllowMultipleValues)
            {
                DropDownList ddlValue = ((DropDownList)control);
                results = ddlValue.SelectedItem.Value + ";#" + ddlValue.SelectedItem.Text;
            }
            else
            {
                HtmlGenericControl div = ((HtmlGenericControl)control);
                CheckBoxList listBox = (CheckBoxList)div.Controls[0];

                foreach (var item in listBox.Items.Cast<ListItem>().Where(p => p.Selected))
                {
                    results += item.Value + ";#" + item.Text + SEPARATOR;
                }
            }
            return results.TrimEnd(SEPARATOR.ToCharArray());
        }

        public static void MoveSelectedItem(ListBox source, ListBox destination)
        {
            List<ListItem> sourceItems = source.Items.Cast<ListItem>().ToList();
            List<ListItem> destinationItems = destination.Items.Cast<ListItem>().ToList();

            foreach (ListItem item in sourceItems)
            {
                if (item.Selected)
                {
                    if (destinationItems.FirstOrDefault(i => i.Value == item.Value) == null)
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
    }
}
