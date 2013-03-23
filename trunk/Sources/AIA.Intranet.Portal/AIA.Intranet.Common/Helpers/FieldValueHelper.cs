using System;
using System.Collections.Generic;
using System.Linq;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;

using AIA.Intranet.Model;
using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Common.Helpers
{
    public class FieldValueHelper
    {
         public static bool ValidateItemProperty(SPListItem item, Guid fieldId, Criteria criteria )
        {
            string itemValueString = item[fieldId].ToString();
            
            switch (item.Fields[fieldId].Type)
            {
                case SPFieldType.User:
                case SPFieldType.Lookup:
                    SPFieldLookup lookupField = (SPFieldLookup)item.Fields[fieldId];
                    
                    if (lookupField.AllowMultipleValues)
                    {
                        SPFieldLookupValueCollection itemValueCol = new SPFieldLookupValueCollection(itemValueString);
                        return itemValueCol.EnhancedCompareTo(criteria);
                    }
                    
                    //the settings can only save the lookup id in case of single lookup
                    SPFieldLookupValue itemFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(itemValueString);
                    return itemFieldValue.EnhancedCompareTo(criteria);
                
                case SPFieldType.Computed:
                case SPFieldType.Note:
                case SPFieldType.Text:
                    return itemValueString.EnhancedCompareTo(criteria);

                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Number:
                    long itemValue;
                    if (long.TryParse(itemValueString, out itemValue))
                    {
                        return itemValue.EnhancedCompareTo(criteria);
                    }
                    break;
                
                case SPFieldType.DateTime:
                    DateTime itemDateTimeValue;
                    if (DateTime.TryParse(itemValueString, out itemDateTimeValue))
                    {
                        return itemDateTimeValue.EnhancedCompareTo(criteria);
                    }
                    break;
                
                default:
                    return itemValueString.EnhancedCompareTo(criteria);

            }
            return false;
        }
        public static bool ValidateItemProperty(SPListItem item, Guid fieldId, Criteria criteria, string itemValueString)
        {
            switch (item.Fields[fieldId].Type)
            {
                case SPFieldType.Boolean:
                    return Boolean.Parse(itemValueString).EnhancedCompareTo(criteria);
                case SPFieldType.User:
                    SPFieldUser userField = (SPFieldUser)item.Fields[fieldId];
                    
                    SPPrincipal spPrincipal = item.ParentList.ParentWeb.Site.FindUserOrSiteGroup(itemValueString);
                    if (userField.AllowMultipleValues)
                    {
                        //SPFieldLookupValueCollection userValues = new SPFieldLookupValueCollection(itemValueString);
                        //return userValues.EnhancedCompareTo(criteria);
                        return false;
                    }
                    SPFieldLookupValue userValue = new SPFieldLookupValue(spPrincipal.ID, spPrincipal.Name);
                    return userValue.EnhancedCompareTo(criteria);

                case SPFieldType.Lookup:
                    SPFieldLookup lookupField = (SPFieldLookup)item.Fields[fieldId];

                    if (lookupField.AllowMultipleValues)
                    {
                        SPFieldLookupValueCollection itemValueCol = new SPFieldLookupValueCollection(itemValueString);
                        return itemValueCol.EnhancedCompareTo(criteria);
                    }
                    SPFieldLookupValue itemFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(itemValueString);
                    return itemFieldValue.EnhancedCompareTo(criteria);

                case SPFieldType.Computed:
                case SPFieldType.Note:
                case SPFieldType.Text:
                    return itemValueString.EnhancedCompareTo(criteria);

                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Number:
                    long itemValue;
                    if (long.TryParse(itemValueString, out itemValue))
                    {
                        return itemValue.EnhancedCompareTo(criteria);
                    }
                    break;

                case SPFieldType.DateTime:
                    DateTime itemDateTimeValue;
                    if (DateTime.TryParse(itemValueString, out itemDateTimeValue))
                    {
                        return itemDateTimeValue.EnhancedCompareTo(criteria);
                    }
                    break;

                default:
                    return itemValueString.EnhancedCompareTo(criteria);

            }
            return false;
        }
    }

    public static class EnhancedCompareToExtentions
    {
        public static bool EnhancedCompareTo(this string itemValue, Criteria criteria)
        {
            switch (criteria.Operator)
            {
                case Operators.Equal:
                    return itemValue.CompareTo(criteria.Value) == 0;
                case Operators.NotEqual:
                    return itemValue.CompareTo(criteria.Value) != 0;
                case Operators.StartsWith:
                    return itemValue.StartsWith(criteria.Value);
                case Operators.Contains:
                    return itemValue.Contains(criteria.Value);
            }
            return false;
        }

        public static bool EnhancedCompareTo(this bool itemValue, Criteria criteria)
        {
            switch (criteria.Operator)
            {
                case Operators.Equal:
                    return itemValue == Convert.ToBoolean(Convert.ToByte(criteria.Value));
                case Operators.NotEqual:
                    return itemValue != Convert.ToBoolean(Convert.ToByte(criteria.Value));
            }
            return false;
        }
        public static bool EnhancedCompareTo(this long itemValue, Criteria criteria)
        {
            long dest = 0; 
            bool ok = long.TryParse(criteria.Value, out dest);
            
            if (!ok) return false;

            switch (criteria.Operator)
            {
                case Operators.Equal:
                    return itemValue == dest;
                case Operators.NotEqual:
                    return itemValue != dest;
                case Operators.GreaterThan:
                    return itemValue > dest;
                case Operators.LessThan:
                    return itemValue < dest;
            }

            return false;
        }

        public static bool EnhancedCompareTo(this DateTime itemValue, Criteria criteria)
        {
            DateTime dest;
            bool ok = DateTime.TryParse(criteria.Value, out dest);
            if (!ok) return false;

            switch (criteria.Operator)
            { 
                case Operators.Equal:
                    return itemValue.CompareTo(dest) == 0;
                
                case Operators.NotEqual:
                    return itemValue.CompareTo(dest) != 0;
                
                case Operators.EarlierThan:
                    return itemValue.CompareTo(dest) < 0;
                
                case Operators.LaterThan:
                    return itemValue.CompareTo(dest) > 0;
            }

            return false;
        }

        public static bool EnhancedCompareTo(this List<SPFieldLookupValue> itemValueCol, Criteria criteria)
        {
            SPFieldLookupValueCollection criteriaValueCol = new SPFieldLookupValueCollection(criteria.Value);
            var query1 = from criteriaLookup in criteriaValueCol
                    join itemLookup in itemValueCol
                    on criteriaLookup.LookupId equals itemLookup.LookupId
                    select new { criteriaLookup.LookupId };

            switch (criteria.Operator)
            {
                case Operators.Equal:
                    return (itemValueCol.Count == criteriaValueCol.Count &&
                        query1.Count() == criteriaValueCol.Count);

                case Operators.NotEqual:
                    return (itemValueCol.Count != criteriaValueCol.Count ||
                        query1.Count() != criteriaValueCol.Count);

                case Operators.Contains:
                    var query2 = from itemLookup in itemValueCol
                            join criteriaLookup in criteriaValueCol
                            on itemLookup.LookupId equals criteriaLookup.LookupId
                            select new { criteriaLookup.LookupId };

                    return (query2.Count() == criteriaValueCol.Count);
            }

            return false;
        }

        public static bool EnhancedCompareTo( this SPFieldLookupValue itemValue, Criteria criteria)
        {
            SPFieldLookupValue criteriaValue = new SPFieldLookupValue(criteria.Value);

            switch (criteria.Operator)
            { 
                case Operators.Contains:
                case Operators.Equal:
                    return itemValue.LookupId == criteriaValue.LookupId;
                case Operators.NotEqual:
                    return itemValue.LookupId != criteriaValue.LookupId;
            }
            return false;
        }

        public static string GetValueAsString(this SPListItem item, string fieldname)
        {
            if (item[fieldname] == null || string.IsNullOrEmpty(item[fieldname].ToString()))
                return null;
            SPField field = item.Fields[fieldname];
            string ReturnValue;
            switch (field.Type)
            {
                case SPFieldType.Lookup:
                    if (((SPFieldLookup)field).AllowMultipleValues == false)
                    {
                        SPFieldLookupValue lookup = new SPFieldLookupValue(item[fieldname].ToString());
                        ReturnValue = lookup.LookupValue;
                    }
                    else
                    {
                        SPFieldLookupValueCollection lookup = new SPFieldLookupValueCollection(item[fieldname].ToString());
                        ReturnValue = lookup[0].LookupValue;
                    }
                    break;
                case SPFieldType.User:
                    if (((SPFieldUser)field).AllowMultipleValues == false)
                    {
                        SPFieldUserValue users = new SPFieldUserValue(item.Web, item[fieldname].ToString());
                        ReturnValue = users.User.Name;
                    }
                    else
                    {
                        SPFieldUserValueCollection users = new SPFieldUserValueCollection(item.Web, item[fieldname].ToString());
                        ReturnValue = users[0].User.Name;
                    }
                    break;
                default:
                    ReturnValue = item[fieldname].ToString().Trim();
                    break;
            }
            return ReturnValue;
        }
    }
}
