using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class Email10ValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            String Email = (String)value;
            bool result = isEmailID(Email);
            if (result != true)
            {
                return new ValidationResult(false, "Enter valid Email.");
            }
            else
            {
                return new ValidationResult(true, "Email Entered is correct.");
            }

        }

        public static bool isEmailID(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

    }

}
