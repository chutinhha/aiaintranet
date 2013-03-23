using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPFieldCollectionExtensions
    {
        public static bool ContainFieldId(this SPFieldCollection fields, Guid fieldId)
        {
            foreach (SPField field in fields)
            {
                if (field.Id == fieldId) return true;
            }
            return false;
        }

        public static List<SPField> Order(this SPFieldCollection fields)
        {
            return fields.Cast<SPField>()
               .OrderBy(ct => ct.Title)
               .ToList<SPField>();
        }

        public static List<SPField> AvailableSearchFields(this SPFieldCollection fields)
        {
            return fields.Cast<SPField>().Where(p => !p.Hidden &&
                   (p.Type != SPFieldType.Computed || p.Id == SPBuiltInFieldId.ContentType) &&
                   p.Title != "Predecessors" &&
                   p.Title != "Related Issues").ToList();
        }

        public static List<SPField> AvailableResultFields(this SPFieldCollection fields)
        {
            List<SPField> listFields = fields.Cast<SPField>().Where(p => !p.Hidden &&
                   (p.Type != SPFieldType.Computed || p.Id == SPBuiltInFieldId.ContentType) &&
                   p.Title != "Predecessors" &&
                   p.Title != "Related Issues").ToList();
            
            SPField docIconField = SPContext.Current.Web.AvailableFields[SPBuiltInFieldId.DocIcon];
            listFields.Add(docIconField);
            return listFields;
        }
        
        public static List<SPField> AvailableFieldsToUpdate(this List<SPField> fields)
        {
            List<SPField> listFieldsReturn = new List<SPField>();
            foreach (SPField field in fields)
            {
                switch (field.Type)
                {
                    case SPFieldType.Boolean:
                    case SPFieldType.Choice:
                    case SPFieldType.Currency:
                    case SPFieldType.DateTime:
                    case SPFieldType.Integer:
                    case SPFieldType.MultiChoice:
                    case SPFieldType.Note:
                    case SPFieldType.Number:
                    case SPFieldType.Text:
                    case SPFieldType.User:
                        listFieldsReturn.Add(field);
                        break;

                    case SPFieldType.Lookup:
                        SPFieldLookup lookup = field as SPFieldLookup;
                        if (!string.IsNullOrEmpty(lookup.LookupList) && string.Compare("lookup.LookupList", "self", true) != 0)
                        {
                            listFieldsReturn.Add(field);
                        }
                        break;
                }
            }
            return listFieldsReturn;
        }

        public static List<SPField> AvailableFieldsToUpdate(this SPFieldCollection fields)
        {
            List<SPField> listFields = fields.Cast<SPField>().Where(p => !p.Hidden &&
                   (p.Type != SPFieldType.Computed || p.Id == SPBuiltInFieldId.ContentType) &&
                   p.Title != "Predecessors" &&
                   p.Title != "Related Issues").ToList();
            return AvailableFieldsToUpdate(listFields);
        }

        public static bool ContainFieldName(this SPFieldCollection fields, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return false;

            foreach (SPField field in fields)
            {
                if (string.Compare(field.Title, fieldName, true) == 0) return true;
            }
            return false;
        }
    }
}
