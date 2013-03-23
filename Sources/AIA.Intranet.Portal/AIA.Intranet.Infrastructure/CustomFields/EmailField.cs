using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Globalization;
using System.Windows.Controls;
using System.Security.Permissions;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class EmailField : SPFieldText
    {

        public EmailField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public EmailField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new EmailFieldControl();
                fieldControl.FieldName = this.InternalName;

                return fieldControl;
            }
        }
        public override string GetValidatedString(object value)
        {
            if ((this.Required == true)
               &&
               ((value == null)
                ||
               ((String)value == "")))
            {
                throw new SPFieldValidationException(this.Title
                    + " must have a value.");
            }
            else
            {



                Email10ValidationRule rule = new Email10ValidationRule();
                ValidationResult result = rule.Validate(value, CultureInfo.InvariantCulture);




                if (!result.IsValid)
                {
                    throw new SPFieldValidationException((String)result.ErrorContent);
                }
                else
                {
                    return base.GetValidatedString(value);
                }
            }
        }
    }
}


