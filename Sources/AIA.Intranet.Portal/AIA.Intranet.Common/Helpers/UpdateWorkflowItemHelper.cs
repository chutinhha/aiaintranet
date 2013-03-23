using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Helpers
{
    public class UpdateWorkflowItemHelper
    {
        /// <summary>
        /// Update SPListItem with correcsponding data parse from actions.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatedField"></param>
        /// <param name="data"></param>
        public static void DoUpdateItem(SPListItem item, SPField updatedField, string data)
        {
            switch (updatedField.Type)
            {
                case SPFieldType.Boolean:
                    item[updatedField.Id] = Convert.ToBoolean(data);
                    break;

                case SPFieldType.File:
                case SPFieldType.Calculated:
                case SPFieldType.Computed:
                case SPFieldType.Currency:
                case SPFieldType.Integer:
                case SPFieldType.Note:
                case SPFieldType.Number:
                case SPFieldType.Text:
                    item[updatedField.Id] = data;
                    break;

                case SPFieldType.Choice:
                    SPFieldChoice fieldChoice = (SPFieldChoice)updatedField;
                    item[updatedField.Id] = data;
                    break;

                case SPFieldType.DateTime:
                    SPFieldDateTime fieldDate = (SPFieldDateTime)updatedField;
                    item[updatedField.Id] = Convert.ToDateTime(data);
                    break;

                case SPFieldType.Lookup:

                    SPFieldLookup fieldLookup = (SPFieldLookup)updatedField;
                    if (fieldLookup.AllowMultipleValues)
                    {
                        SPFieldLookupValueCollection multiValues = new SPFieldLookupValueCollection();
                        foreach (var s in data.Split("|".ToCharArray()))
                        {
                            multiValues.Add(new SPFieldLookupValue(s));
                        }
                        item[updatedField.Id] = multiValues;
                    }
                    else
                    {
                        //int id = fieldLookup.GetLookupIdFromValue(data);

                        SPFieldLookupValue singleLookupValue = new SPFieldLookupValue(data);
                        item[updatedField.Id] = singleLookupValue;
                    }
                    break;

                case SPFieldType.MultiChoice:
                    SPFieldMultiChoice fieldMultichoice = (SPFieldMultiChoice)updatedField;

                    string[] items = data.Split("|".ToCharArray());
                    SPFieldMultiChoiceValue values = new SPFieldMultiChoiceValue();
                    foreach (string choice in items)
                    {
                        values.Add(choice);
                    }

                    item[updatedField.Id] = values;

                    break;

                case SPFieldType.User:
                    SPFieldUser fieldUser = (SPFieldUser)updatedField;

                    SPFieldUserValueCollection fieldValues = new SPFieldUserValueCollection();
                    string[] entities = data.Split("|".ToCharArray());

                    foreach (string entity in entities)
                    {
                        SPUser user = item.Web.EnsureUser(entity.Split(";#".ToCharArray())[2]);
                        if (user != null)
                            fieldValues.Add(new SPFieldUserValue(item.Web, user.ID, user.Name));
                    }

                    item[updatedField.Id] = fieldValues;
                    break;

                case SPFieldType.Invalid:
                    if (string.Compare(updatedField.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                    {
                        item[updatedField.Id] = data;
                    }
                    break;
            }
        }
    }
}
