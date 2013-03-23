using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace AIA.Intranet.Common.Extensions
{
    public static class ListFieldIteratorExtensions
    {
        public static FormField GetFormField(this ListFieldIterator listFieldIterator, string fieldName)
        {
            return GetFormField(listFieldIterator, GetFormFields(listFieldIterator), fieldName);
        }


        public static FormField GetFormField(this ListFieldIterator listFieldIterator, List<FormField> formFields, string fieldName)
        {
            FormField formField = (from form in formFields
                                   where form.FieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase)
                                   select form).FirstOrDefault();

            if (formField == null)
            {
                throw new Exception("Could not find form field: " + fieldName);
            }

            return formField;
        }

        public static List<FormField> GetFormFields(this ListFieldIterator listFieldIterator)
        {
            if (listFieldIterator == null)
            {
                return null;
            }

            return FindFieldFormControls(listFieldIterator);
        }

        private static List<FormField> FindFieldFormControls(System.Web.UI.Control root)
        {
            List<FormField> baseFieldControls = new List<FormField>();

            foreach (System.Web.UI.Control control in root.Controls)
            {
                if (control is FormField && control.Visible)
                {
                    FormField formField = control as FormField;
                    if (formField.Field.FieldValueType == typeof(DateTime))
                    {
                        HandleDateField(formField);
                    }

                    baseFieldControls.Add(formField);
                }
                else
                {
                    baseFieldControls.AddRange(FindFieldFormControls(control));
                }
            }

            return baseFieldControls;
        }

        private static void HandleDateField(FormField formField)
        {
            if (formField.ControlMode == SPControlMode.Display)
            {
                return;
            }

            Control dateFieldControl = formField.Controls[0];
            if (dateFieldControl.Controls.Count > 0)
            {
                DateTimeControl dateTimeControl = (DateTimeControl)dateFieldControl.Controls[0].Controls[1];
                TextBox dateTimeTextBox = dateTimeControl.Controls[0] as TextBox;
                if (dateTimeTextBox != null)
                {
                    if (!string.IsNullOrEmpty(dateTimeTextBox.Text))
                    {
                        formField.Value = DateTime.Parse(dateTimeTextBox.Text, CultureInfo.CurrentCulture);
                    }
                }
            }
        }
    }
}
