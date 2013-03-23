using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPFieldExtensions
    {
        public static SPFieldLookupValue ResolveLookupText(this SPFieldLookup field, string unresolvedText)
        {
            SPFieldLookupValue lookupValue = null;
            using (SPWeb web = SPContext.Current.Site.OpenWeb(field.LookupWebId))
            {
                SPList list = web.Lists[new Guid(field.LookupList)];
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><Eq><FieldRef Name=\"{0}\"/><Value Type=\"Text\">{1}</Value></Eq></Where>", field.LookupField, unresolvedText);
                SPListItemCollection items = list.GetItems(query);
                if (items.Count == 0) return null;
                lookupValue = new SPFieldLookupValue(items[0].ID, items[0][field.LookupField] == null ? "" : items[0][field.LookupField].ToString());
            }

            return lookupValue;
        }

        public static int GetLookupIdFromValue(this SPFieldLookup field, string fieldValue)
        {
            int ret = 0;
            try //assume fieldValue has both ID and Value
            {
                SPFieldLookupValue pairValue = new SPFieldLookupValue(fieldValue);

                if (pairValue != null && pairValue.LookupId != 0)
                    return pairValue.LookupId;
            }
            catch
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPWeb web = SPContext.Current.Site.OpenWeb(field.LookupWebId))
                    {
                        SPList list = web.Lists.GetList(new Guid(field.LookupList), false);
                        SPListItemCollection founds = list.FindItems(field.LookupField, fieldValue);
                        if (founds.Count > 0)
                            ret = founds[0].ID;

                    }
                });
            }
            return ret;
        }

        public static void UpdateTargetList(this SPFieldLookup field, string targetListId)
        {
            try
            {
                if (!field.SchemaXml.Contains(targetListId))
                {
                    field.SchemaXml = ReplaceXmlAttributeValue(field.SchemaXml, "List", targetListId);
                    field.Update();
                }
            }
            catch {
                CCIUtility.LogInfo("The field : " + field.Title + " doesn't exit in target list id" + targetListId, "AIA.Intranet.Common");
                /*field is not existed*/
                
                }
        }

        private static string ReplaceXmlAttributeValue(string xml, string attributeName, string value)
        {
            int indexOfAttributeName = xml.IndexOf(attributeName, StringComparison.CurrentCultureIgnoreCase);
            if (indexOfAttributeName == -1)
            {
                throw new ArgumentOutOfRangeException("attributeName", string.Format("Attribute {0} not found in source xml", attributeName));
            }

            int indexOfAttibuteValueBegin = xml.IndexOf('"', indexOfAttributeName);
            int indexOfAttributeValueEnd = xml.IndexOf('"', indexOfAttibuteValueBegin + 1);

            return xml.Substring(0, indexOfAttibuteValueBegin + 1) + value + xml.Substring(indexOfAttributeValueEnd);
        }

        public static bool IsSystemField(this SPField field)
        {
            return field.Id == SPBuiltInFieldId._Author
                || field.Id == SPBuiltInFieldId.Created_x0020_By
                || field.Id == SPBuiltInFieldId.Created
                || field.Id == SPBuiltInFieldId.Created_x0020_Date
                || field.Id == SPBuiltInFieldId.Last_x0020_Modified
                || field.Id == SPBuiltInFieldId.Modified_x0020_By
                || field.Id == SPBuiltInFieldId.Modified
                || field.Id == SPBuiltInFieldId.ID
                || field.Id == SPBuiltInFieldId.FileType
                || field.Id == SPBuiltInFieldId._CopySource
                || field.Id == SPBuiltInFieldId._CheckinComment
                || field.Id == SPBuiltInFieldId.LinkTitle
                || field.Id == SPBuiltInFieldId.ListType
                || field.Id == SPBuiltInFieldId.Attachments
                || field.Id == SPBuiltInFieldId.Edit
                || field.Id == SPBuiltInFieldId.FolderChildCount
                || field.Id == SPBuiltInFieldId.ItemChildCount
                || field.Id == SPBuiltInFieldId._Version
                || field.Id == SPBuiltInFieldId.CheckoutUser
                || field.Id == SPBuiltInFieldId.File_x0020_Size
                //all for custom list field
                || field.Id == new Guid("{dce8262a-3ae9-45aa-aab4-83bd75fb738a}") //Create By   
                || field.Id == new Guid("{1df5e554-ec7e-46a6-901d-d85a3881cb18}") //Modified By 
                || field.Id == new Guid("{d31655d1-1d5b-4511-95a1-7a09e9b75bf2}") //Version
                || field.Id == new Guid("{081c6e4c-5c14-4f20-b23e-1a71ceb6a67c}") //Type
                || field.Id == new Guid("{bc91a437-52e7-49e1-8c4e-4698904b2b6d}"); //Link to Title
        }

        public static Control SetValueToControl(this SPFieldCalculated calcField, Control control, string value)
        {
            switch (calcField.OutputType)
            {
                case SPFieldType.DateTime:
                    DateTimeControl dtcValueDate = (DateTimeControl)control;
                    dtcValueDate.SelectedDate = Convert.ToDateTime(value).ToUniversalTime();
                    break;

                case SPFieldType.Boolean:
                    CheckBox chkField = (CheckBox)control;
                    chkField.Checked = Convert.ToBoolean(Convert.ToByte(value));
                    break;

                default:
                    TextBox txtInput = (TextBox)control;
                    txtInput.Text = value;
                    break;

            }
            return control;
        }

        public static string GetFieldValueAsTextEx(this SPFieldCalculated calcField, Control control, bool ignoreValidation)
        {
            string value = string.Empty;
            switch (calcField.OutputType)
            {
                case SPFieldType.Number:
                case SPFieldType.Currency:
                     TextBox txtNumber = (TextBox)control;
                     if (!ignoreValidation && string.IsNullOrEmpty(txtNumber.Text)) break;
                        value = txtNumber.Text;
                    break;

                case SPFieldType.DateTime:
                    DateTimeControl dtcValueDate = (DateTimeControl)control;
                    if (!ignoreValidation && dtcValueDate.IsDateEmpty) break;
                    value = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtcValueDate.SelectedDate);
                    break;

                case SPFieldType.Boolean:
                    CheckBox chkField = (CheckBox)control;
                    value = chkField.Checked ? "1" : "0";
                    break;

                default:
                    TextBox txtString = (TextBox)control;
                    if (!ignoreValidation && string.IsNullOrEmpty(txtString.Text)) break;
                    value = txtString.Text;
                    break;
            }
            return value;
        }

        public static Control GetFieldControl(this SPFieldCalculated calcField)
        {
            Control control = null;
            switch (calcField.OutputType)
            {
                case SPFieldType.Number:
                case SPFieldType.Currency:
                    control = new TextBox() { CssClass = "ms-long" };
                    break;

                case SPFieldType.DateTime:
                    DateTimeControl dtcValueDate = new DateTimeControl();
                    dtcValueDate.DateOnly = (calcField.DateFormat == SPDateTimeFieldFormatType.DateOnly);
                    control = dtcValueDate;
                    break;

                case SPFieldType.Boolean:
                    control = new CheckBox();
                    break;

                default:
                   control = new TextBox() { CssClass = "ms-long" };
                    break;
            }
            return control;
        }

        public static string FormatValueAsText(this SPFieldCalculated calcField, string rawValue)
        {
            string result = rawValue;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            switch (calcField.OutputType)
            {
                case SPFieldType.Number:
                    try
                    {
                        var numberValue = Convert.ToDouble(rawValue);
                        result = SPFieldNumber.GetFieldValueAsText(numberValue, currentCulture, calcField.ShowAsPercentage, calcField.DisplayFormat);
                    }
                    catch { result = string.Empty; }
                    break;

                case SPFieldType.DateTime:
                    try
                    {
                        var dateValue = Convert.ToDateTime(rawValue);
                        result = SPFieldDateTime.GetFieldValueAsText(dateValue, SPContext.Current.Web, calcField.DateFormat == SPDateTimeFieldFormatType.DateTime ? SPDateFormat.DateTime : SPDateFormat.DateOnly);
                    }
                    catch { result = string.Empty; }
                    break;

                case SPFieldType.Currency:
                    try
                    {
                        double data = Convert.ToDouble(rawValue, CultureInfo.InvariantCulture);
                        result = SPFieldCurrency.GetFieldValueAsText(data, currentCulture, calcField.CurrencyLocaleId, calcField.DisplayFormat);
                    }
                    catch { result = string.Empty; }
                    break;

                case SPFieldType.Boolean:
                    try
                    {
                        result = Convert.ToBoolean(Convert.ToByte(rawValue)).ToString();
                    }
                    catch { }
                    break;

                default:
                    break;
            }
            return result;
        }
           
        internal static int StsBinaryCompareIndexOf(string str1, string str2)
        {
            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(str1, str2, CompareOptions.Ordinal);
        }

        internal static string GetCalcLibErrorString(string strErrorCode)
        {
            switch (strErrorCode)
            {
                case "0":
                    return SPResource.GetString("CalcLibErrorDiv0", new object[0]);

                case "1":
                    return SPResource.GetString("CalcLibErrorValue", new object[0]);

                case "3":
                    return SPResource.GetString("CalcLibErrorNull", new object[0]);

                case "4":
                    return SPResource.GetString("CalcLibErrorCirc", new object[0]);

                case "5":
                    return SPResource.GetString("CalcLibErrorNum", new object[0]);

                case "6":
                    return SPResource.GetString("CalcLibErrorRef", new object[0]);

                case "7":
                    return SPResource.GetString("CalcLibErrorNA", new object[0]);
            }
            return SPResource.GetString("CalcLibErrorName", new object[0]);
        }

        private static string GetFormattedErrorValue(string strType, string strData)
        {
            if (strType == "ERROR")
            {
                return GetCalcLibErrorString(strData);
            }
            return SPHttpUtility.HtmlEncode(SPResource.GetString("CalcLibErrorName", new object[0]));
        }
        
        internal static string FormatValue(double data, NumberFormatInfo nfi)
        {
            return data.ToString("C", nfi);
        }

        public static  string GetFieldValueAsTextOrHtml( this SPFieldCalculated calcField, object value, bool asHtml)
        {
            if (value == null)
            {
                return string.Empty;
            }

          
            string strType = string.Empty;
            string str2 = string.Empty;
            string str3 = value as string;
            if (str3 == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            str2 = str3;
            int length = StsBinaryCompareIndexOf(str2, ";#");
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            strType = str2.Substring(0, length).ToUpper(CultureInfo.InvariantCulture);
            str2 = str2.Substring(length + ";#".Length);
            if (((calcField.OutputType == SPFieldType.Invalid) || (calcField.OutputType == SPFieldType.Error)) || (strType == "ERROR"))
            {
                string formattedErrorValue = GetFormattedErrorValue(strType, str2);
                if (asHtml)
                {
                    return SPHttpUtility.HtmlEncode(formattedErrorValue);
                }
                return formattedErrorValue;
            }
            try
            {
                bool flag = false; ;
                switch (strType)
                {
                    case "FLOAT":
                        {
                            //double data = Convert.ToDouble(str2, CultureInfo.InvariantCulture);
                            //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                            //if (calcField.OutputType == SPFieldType.Currency)
                            //{
                            //    string valueToEncode = FormatValue(data, calcField.Currency..DisplayFormat);
                            //    if (asHtml)
                            //    {
                            //        valueToEncode = SPHttpUtility.HtmlEncode(valueToEncode);
                            //    }
                            //    return valueToEncode;
                            //}
                            //if (asHtml)
                            //{
                            //    return SPFieldNumber.GetFieldValueAsHtml(data, currentCulture, calcField.ShowAsPercentage, calcField.DisplayFormat);
                            //}
                            //return SPFieldNumber.GetFieldValueAsText(data, currentCulture, calcField.ShowAsPercentage, calcField.DisplayFormat);
                        }
                        break;
                    case "DATETIME":
                        {
                            //SPDateFormat dateTime;
                            //if (base.IsXLVWPConnection)
                            //{
                            //    return str2;
                            //}
                            //DateTime time = SPUtility.CreateSystemDateTimeFromXmlDataDateTimeFormat(str2);
                            //if (this.DateFormat == SPDateTimeFieldFormatType.DateTime)
                            //{
                            //    dateTime = SPDateFormat.DateTime;
                            //}
                            //else
                            //{
                            //    dateTime = SPDateFormat.DateOnly;
                            //}
                            //if (asHtml)
                            //{
                            //    return SPFieldDateTime.GetFieldValueAsHtml(time, base.Fields.Web, dateTime);
                            //}
                            //return SPFieldDateTime.GetFieldValueAsText(time, base.Fields.Web, dateTime);
                        }
                        break;
                    default:
                        if (!(strType == "BOOLEAN"))
                        {
                            goto Label_01FA;
                        }
                        switch (str2)
                        {
                            case "TRUE":
                            case "-1":
                            case "1":
                                flag = true;
                                break;
                        }
                        flag = false;
                        break;
                }
                //if (asHtml)
                //{
                //    return SPFieldBoolean.GetFieldValueAsHtml(flag);
                //}
                return SPFieldBoolean.GetFieldValueAsText(flag);
            Label_01FA:
                if (asHtml)
                {
                    return SPHttpUtility.HtmlEncode(str2);
                }
                return str2;
            }
            catch (ArgumentOutOfRangeException)
            {
                if (asHtml)
                {
                    return SPHttpUtility.HtmlEncode(str2);
                }
                return str2;
            }

            //return string.Empty;
        }

        public static void UpdateImageField(this SPField field, SPWeb web, SPList list)
        {
            //if (string.IsNullOrEmpty(lookupField.LookupList))
            //{
            //    lookupField.LookupWebId = web.ID;
            //    lookupField.LookupList = list.ID.ToString();
            //}
            //else
            //{
            //    field.SchemaXml =
            //        field.SchemaXml
            //            .ReplaceXmlAttributeValue("PictureLibraryId", list.ID.ToString())
            //            .ReplaceXmlAttributeValue("WebId", web.ID.ToString());
            //}

            field.SchemaXml =
                   field.SchemaXml
                       .EnsureXmlAttribute("PictureLibraryId", list.ID.ToString())
                       .EnsureXmlAttribute("WebId", web.ID.ToString());
                       


            field.Update(true);
        }

        public static void UpdateProperties(this SPField field, System.Collections.Generic.Dictionary<string, object> props)
        {
            string schema = field.SchemaXml;

            foreach (var item in props)
            {
                schema = schema.EnsureXmlAttribute(item.Key, item.Value);
            }

            field.SchemaXml = schema;

            field.Update(true);
        }

    }
}
