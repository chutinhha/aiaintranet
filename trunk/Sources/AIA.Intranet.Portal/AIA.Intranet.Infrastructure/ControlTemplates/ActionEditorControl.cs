using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Workflow;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections;
using AIA.Intranet.Model;
//using FirstData.Fields;

namespace AIA.Intranet.Infrastructure.Controls
{
    public class ActionEditorControl: UserControl
    {
        private const string GENERIC_CONTROL_ID="GenericFieldControl";
        private const string SEPARATOR = "|";
        public ActionEditorControl()
        {
            ReadOnly = false;
            IsFirstLoad = false;
        }

        public TaskActionSettings Action {get; set; }
        public bool ReadOnly { get; set; }
        public bool IsFirstLoad { get; set; }
        public string ContentTypeId { get; set; }
        public string ListId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if(string.IsNullOrEmpty(ContentTypeId))
            ContentTypeId = Request["ctype"];
            base.OnInit(e);
        }

        public virtual TaskActionSettings GetAction() { return null; }

        protected SPContentType GetContentType()
        {
            SPContentType contentType = null;
            SPList list = SPContext.Current.List;

            if (list != null)
            {
                contentType = list.ContentTypes[new SPContentTypeId(ContentTypeId)];
            }
            else
            {
                contentType = SPContext.Current.Web.FindContentType(new SPContentTypeId(ContentTypeId));
            }

            return contentType;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (ReadOnly)
            {
                LockControl(this);
            }
            base.OnLoad(e);
        }

        private void LockControl(Control control)
        {

            foreach (Control ctr in control.Controls)
            {
                LockControl(ctr);
            }
            try
            {

                WebControl webControl = control as WebControl;
                webControl.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }
        protected void SetDropDownListValue(DropDownList control, string value)
        {
            ListItem item = control.Items.FindByValue(value);
            if (item != null)
                control.SelectedValue = item.Value;
        }

        public Control BuildValueSelectorControl(SPField field, string displayValue)
        {
            Control control = null;
            if (field == null) { control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString() }; }
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
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString() };
                        break;
                    case SPFieldType.Choice:
                        SPFieldChoice fieldChoice = (SPFieldChoice)field;
                        DropDownList ddlValueChoice = new DropDownList();

                        foreach (string value in fieldChoice.Choices)
                        {
                            ddlValueChoice.Items.Add(new ListItem(value));
                        }

                        ddlValueChoice.Items.Insert(0, string.Empty);
                        ddlValueChoice.SelectedIndex = ddlValueChoice.Items.IndexOf(ddlValueChoice.Items.FindByValue(displayValue));
                        control = ddlValueChoice;
                        break;

                    case SPFieldType.DateTime:
                        SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                        DateTimeControl dtcValueDate = new DateTimeControl();
                        dtcValueDate.DateOnly = (fieldDate.DisplayFormat == SPDateTimeFieldFormatType.DateOnly);
                        if (!string.IsNullOrEmpty(displayValue.ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(displayValue.ToString(), out date))
                            {
                                dtcValueDate.SelectedDate = Convert.ToDateTime(date).ToUniversalTime();
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
                            string[] arr = displayValue.Split(SEPARATOR.ToCharArray());

                            item.Selected = arr.Contains(value);

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
                        PeopleEditor peoValue = new PeopleEditor() { CssClass = "ms-long" };
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
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue };
                        break;
                }
            }
            control.ID = GENERIC_CONTROL_ID;
            return control;
        }

        public Control BuildValueSelectorControlEx(SPField field, string displayValue)
        {
            Control control = null;
            if (field == null) { control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString() }; }
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
                    case SPFieldType.Number:
                    case SPFieldType.Text:
                    case SPFieldType.URL:                    
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString() };                        
                        break;
                    case SPFieldType.Note:
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString(), TextMode = TextBoxMode.MultiLine };                        
                        break;
                    //case SPFieldType.Invalid:
                    //    if (string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                    //    {                            
                    //        LookupFieldWithPicker lookupfield = (LookupFieldWithPicker)field;
                    //        if (!lookupfield.AllowMultipleValues)
                    //        {                                
                    //            LookupFieldWithPickerControl lookupcontrol = new LookupFieldWithPickerControl() { CssClass = "ms-long" };
                    //            lookupcontrol.ControlMode = SPControlMode.Edit;
                    //            lookupcontrol.ListId = lookupfield.ParentList.ID;
                    //            lookupcontrol.FieldName = lookupfield.Title;
                    //            lookupcontrol.Value = displayValue;
                    //            control = lookupcontrol;
                    //        }
                    //        else
                    //        {                                
                    //            FieldMultiLookupWithPickerControl lookupcontrol = new FieldMultiLookupWithPickerControl() { CssClass = "ms-long" };
                    //            lookupcontrol.ControlMode = SPControlMode.Edit;
                    //            lookupcontrol.ListId = lookupfield.ParentList.ID;
                    //            lookupcontrol.FieldName = lookupfield.Title;
                    //            lookupcontrol.Value = displayValue;
                    //            control = lookupcontrol;
                    //        }
                    //    }
                   //     else
                    //        control = new TextBox() { CssClass = "ms-long", Text = displayValue.ToString() };
                    //    break;

                    case SPFieldType.Choice:
                        SPFieldChoice fieldChoice = (SPFieldChoice)field;
                        DropDownList ddlValueChoice = new DropDownList();

                        foreach (string value in fieldChoice.Choices)
                        {
                            ddlValueChoice.Items.Add(new ListItem(value));
                        }

                        ddlValueChoice.Items.Insert(0, string.Empty);
                        ddlValueChoice.SelectedIndex = ddlValueChoice.Items.IndexOf(ddlValueChoice.Items.FindByValue(displayValue));
                        control = ddlValueChoice;
                        break;

                    case SPFieldType.DateTime:
                        SPFieldDateTime fieldDate = (SPFieldDateTime)field;
                        DateTimeControl dtcValueDate = new DateTimeControl();
                        dtcValueDate.EnableViewState = true;
                        dtcValueDate.DateOnly = (fieldDate.DisplayFormat == SPDateTimeFieldFormatType.DateOnly);
                        if (!string.IsNullOrEmpty(displayValue.ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(displayValue.ToString(), out date))
                            {
                                dtcValueDate.SelectedDate = Convert.ToDateTime(date).ToUniversalTime();
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
                            string[] arr = displayValue.Split(SEPARATOR.ToCharArray());

                            item.Selected = arr.Contains(value);

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
                        PeopleEditor peoValue = new PeopleEditor() { CssClass = "ms-long" };
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
                        control = new TextBox() { CssClass = "ms-long", Text = displayValue };
                        break;
                }
            }
            control.ID = GENERIC_CONTROL_ID;
            return control;
        }

        private Control generateLookupControl(Control control, SPFieldLookup fieldLookup, string displayValue)
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
                string[] arr = displayValue.Split(";#".ToCharArray());

                SetDropDownListValue(ddlValueLookup, arr[0]);
                control = ddlValueLookup;
            }
            else
            {
                CheckBoxList chkValueMultiLookup = new CheckBoxList();
                string[] arr = displayValue.Split(SEPARATOR.ToCharArray());
                
                foreach (KeyValuePair<string, string> k in lookupValues)
                {
                    ListItem item = new ListItem() {Value = k.Key, Text = k.Value, Selected = arr.Contains(k.Key + ";#" + k.Value) };
                    chkValueMultiLookup.Items.Add(item);
                }
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("style", "overflow: auto;height:100px;width:100%;");                
                div.Controls.Add(chkValueMultiLookup);

                control = div;
            }
            return control;
        }

        private Dictionary<string, string> getLookupValues(SPFieldLookup fieldLookup)
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
                        if (item[fieldLookup.LookupField]!=null)
                            ret.Add(item.ID.ToString(), item[fieldLookup.LookupField].ToString());
                }
            return ret;
        }

        public string GetGenericControlValue()
        {            
            var key = Request.Params.AllKeys.Where(p => p.EndsWith(GENERIC_CONTROL_ID)).FirstOrDefault();
            if (string.IsNullOrEmpty(key)) return string.Empty;
            return Request[key];
        }

        internal string GetControlValue(Control control, SPField field)
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
                    PeopleEditor peoValue = (PeopleEditor) control;
                    string arr = "";
                    foreach (PickerEntity entity in peoValue.ResolvedEntities)
                    {
                        if (entity.EntityData[PeopleEditorEntityDataKeys.PrincipalType].ToString() == "User")
                        {
                            arr+= entity.EntityData["SPUserID"] + ";#" + entity.Key + SEPARATOR;
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

        internal string GetControlValueEx(Control control, SPField field)
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
                    result = ((TextBox)control).Text.Trim();
                    break;
                //case SPFieldType.Invalid:
                //    if (string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                //    {
                //        LookupFieldWithPicker lookupfield = (LookupFieldWithPicker)field;
                //        if (!lookupfield.AllowMultipleValues)
                //            result = ((LookupFieldWithPickerControl)control).Value == null ? string.Empty : ((LookupFieldWithPickerControl)control).Value.ToString();
                //        else
                //            result = ((FieldMultiLookupWithPickerControl)control).Value == null ? string.Empty : ((FieldMultiLookupWithPickerControl)control).Value.ToString();
                //    }
                //    else
                //        result = ((TextBox)control).Text.Trim();
                //    break;

                case SPFieldType.Choice:
                    SPFieldChoice fieldChoice = (SPFieldChoice)field;

                    DropDownList ddlValueChoice = ((DropDownList)control);
                    result = ddlValueChoice.SelectedValue;
                    break;

                case SPFieldType.DateTime:
                    SPFieldDateTime fieldDate = (SPFieldDateTime)field;

                    DateTimeControl dtcValueDate = ((DateTimeControl)control);
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

        private string GetValueFromControlLookup(Control control, SPFieldLookup fieldLookup)
        {
            string results = string.Empty;
            if (!fieldLookup.AllowMultipleValues)
            {
                DropDownList ddlValue = ((DropDownList)control);
                results = ddlValue.SelectedItem.Value + ";#" + ddlValue.SelectedItem.Text;
            }
            else
            {
                HtmlGenericControl div =((HtmlGenericControl)control);
                CheckBoxList listBox = (CheckBoxList)div.Controls[0];

                foreach (var item in listBox.Items.Cast<ListItem>().Where(p=>p.Selected))
                {
                    results += item.Value + ";#" + item.Text + SEPARATOR;
                }
            }
            return results.TrimEnd(SEPARATOR.ToCharArray());
        }
    }
}
