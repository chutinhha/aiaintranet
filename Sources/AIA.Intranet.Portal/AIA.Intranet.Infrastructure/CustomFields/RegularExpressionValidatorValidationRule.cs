using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Windows.Controls;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class RegularExpressionValidatorValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Expression
            // 1: RegEx
            // 2: Field value
            // 3: Error message
            Triplet myField = (Triplet)value;

            //check arguments
            if (String.IsNullOrEmpty(Convert.ToString(myField.First)))
            {
                throw new ArgumentException("No regular expression provided");
            }

            if (String.IsNullOrEmpty(Convert.ToString(myField.Second)))
            {
                throw new ArgumentException("No expression to validate provided");
            }

            if (String.IsNullOrEmpty(Convert.ToString(myField.Third)))
            {
                throw new ArgumentException("No error message provided");
            }

            // Perform the validation rule
            Regex rxValidationRegEx = new Regex(Convert.ToString(myField.First));
            if (!rxValidationRegEx.IsMatch(Convert.ToString(myField.Second)))
            {
                return new ValidationResult(false, myField.Third.ToString());
            }
            else
            {
                return new ValidationResult(true, "OK!");
            }
        }
    }
}
