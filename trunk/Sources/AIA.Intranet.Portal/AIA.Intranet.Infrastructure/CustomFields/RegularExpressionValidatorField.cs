using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using System.Globalization;
using System.Web.UI;
using System.Windows.Controls;

namespace AIA.Intranet.Infrastructure.CustomFields
{
   
    public class RegularExpressionValidatorField: SPFieldText
    {
         public class Properties
        {
             public static string RegexValidation = "RegexValidation";

        }
        private static Dictionary<int, string> customProperties = new Dictionary<int, string>();

        public RegularExpressionValidatorField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            Init();
        }

       
        public RegularExpressionValidatorField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            Init();
        }
      
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new RegularExpressionValidatorFieldControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetValidatedString(object value)
        {
            // Check value
            if ((this.Required == true)
                && ((value == null)
                || ((String)value == "")))
            {
                throw new SPFieldValidationException(this.Title + " cannot be empty.");
            }

            // Check default arguments
            if (string.IsNullOrEmpty(Convert.ToString(this.RegexValidation)))
            {
                throw new SPFieldValidationException(this.Title + " has not regular expression filled out (set in the design mode)");
            }
            if (string.IsNullOrEmpty(Convert.ToString(this.GetCustomProperty("myErrorMessage"))))
            {
                throw new SPFieldValidationException(this.Title + " has not error message filled out (set in the design mode)");
            }

            // Fire the validation rule if field is not null
            if (value != null && (String)value != "")
            {
                RegularExpressionValidatorValidationRule rule = new RegularExpressionValidatorValidationRule();
                Triplet myField = new Triplet(this.GetCustomProperty(Properties.RegexValidation), value, this.GetCustomProperty("myErrorMessage"));
                ValidationResult result = rule.Validate(myField, CultureInfo.InvariantCulture);

                // Error on validation
                if (!result.IsValid)
                {
                    throw new SPFieldValidationException((String)result.ErrorContent);
                }
            }
            return base.GetValidatedString(value);
        }

        private void Init()
        {
            this.RegexValidation = this.GetCustomProperty(Properties.RegexValidation) + "";

        }
        private string regexValidation;

        public string RegexValidation
        {
            get
            {
                return customProperties.ContainsKey(RegexValidationContextId) ? customProperties[RegexValidationContextId] : regexValidation;
                //return bool.Parse(svalue);
            }
            set
            {
                this.regexValidation = value.ToString();
            }
        }

        public void UpdateCustomProperty(string property, string value)
        {
            customProperties[GetCatchedId(property)] = value.ToString();
        }

        public int GetCatchedId(string name)
        {
            return name.GetHashCode();
        }
        public int RegexValidationContextId
        {
            get
            {
                return GetCatchedId(Properties.RegexValidation);
            }
        }
        public override void Update()
        {
            this.SetCustomProperty(Properties.RegexValidation, this.RegexValidation.ToString());

            base.Update();
            if (customProperties.ContainsKey(RegexValidationContextId))
                customProperties.Remove(RegexValidationContextId);
        }
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
            
        }


    }
}
