using System.Collections.Generic;
using Microsoft.SharePoint;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Helpers
{
    public class OperatorsHelper
    {

        public static List<Operators> GetOperators(SPFieldType fieldType)
        {
            switch (fieldType)
            {
                case SPFieldType.Computed:
                case SPFieldType.Note:
                case SPFieldType.Text:
                case SPFieldType.File:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.Contains, 
                        Operators.StartsWith
                    };


                case SPFieldType.Lookup:
                case SPFieldType.User:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.Contains, 
                    };

                case SPFieldType.MultiChoice:
                    return new List<Operators>(){ 
                        Operators.NotEqual, 
                        Operators.Contains, 
                    };

                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Number:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.GreaterThan, 
                        Operators.LessThan
                    };
                case SPFieldType.DateTime:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.EarlierThan,
                        Operators.LaterThan
                    };
            }
            return new List<Operators>() { Operators.Equal, Operators.NotEqual };
        }

        public static Dictionary<Operators, string> OperatorDisplayNames =
           new Dictionary<Operators, string>{  {Operators.Equal, "Equals"},
                                                {Operators.NotEqual, "Not Equals"},
                                                {Operators.GreaterThan, "Greater Than"},
                                                {Operators.LessThan, "Less Than"},
                                                {Operators.StartsWith, "Starts With"},
                                                {Operators.Contains, "Contains"},
                                                {Operators.EarlierThan, "Earlier Than"},
                                                {Operators.LaterThan, "Later Than"}};

        public static List<Operators> GetOperators(SPField field)
        {
            SPFieldType type = field.Type;
            
            if (type == SPFieldType.Calculated)
            {
                SPFieldCalculated calcField = (SPFieldCalculated)field;
                switch (calcField.OutputType)
                {
                    case SPFieldType.Number:
                    case SPFieldType.Currency:
                        type = SPFieldType.Number;
                        break;
                    case SPFieldType.Boolean:
                        type = SPFieldType.Boolean;
                        break;
                    case SPFieldType.DateTime:
                        type = SPFieldType.DateTime;
                        break;
                    default:
                        type = SPFieldType.Text;
                        break;
                }
            }

            switch (type)
            {
                case SPFieldType.Computed:
                    if (string.Equals(field.StaticName,"ContentType"))
                        return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual,                         
                        Operators.StartsWith
                    };
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.Contains, 
                        Operators.StartsWith,
                        Operators.IsNull
                    };
                case SPFieldType.Note:
                case SPFieldType.Text:
                case SPFieldType.File:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.Contains, 
                        Operators.StartsWith,
                        Operators.IsNull
                    };


                case SPFieldType.Lookup:
                    SPFieldLookup lookupField = (SPFieldLookup)field;
                    if (lookupField.AllowMultipleValues)
                    {
                        return new List<Operators>() { Operators.NotEqual, Operators.Contains, Operators.IsNull };
                    }
                    break;

                case SPFieldType.User:
                    SPFieldUser userField = (SPFieldUser)field;
                    if (userField.AllowMultipleValues)
                    {
                        return new List<Operators>() { Operators.NotEqual, Operators.Contains, Operators.IsNull };
                    }
                    break;

                case SPFieldType.MultiChoice:
                    return new List<Operators>(){ 
                        Operators.NotEqual, 
                        Operators.Contains, 
                        Operators.IsNull
                    };

                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Number:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.GreaterThan, 
                        Operators.LessThan, 
                        Operators.IsNull
                    };
                case SPFieldType.DateTime:
                    return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.EarlierThan,
                        Operators.LaterThan, 
                        Operators.IsNull
                    };

                case SPFieldType.Invalid:
                    if (string.Compare(field.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                    {
                        return new List<Operators>(){ 
                        Operators.Equal, 
                        Operators.NotEqual, 
                        Operators.Contains, 
                        Operators.StartsWith, 
                        Operators.IsNull
                        };
                    }
                    break;
            }

            return new List<Operators>() { Operators.Equal, Operators.NotEqual, Operators.IsNull };
        }
    }
}
