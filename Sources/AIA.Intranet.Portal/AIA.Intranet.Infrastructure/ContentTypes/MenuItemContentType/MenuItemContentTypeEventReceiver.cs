using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities;
using System.Collections.Generic;
using System.Linq.Expressions;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("dd88fee1-d7c4-455b-bd33-edc39935ffd7")]
    public class MenuItemContentTypeEventReceiverEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Asynchronous After event that occurs after a new item has been added to its containing object.
        /// </summary>
        /// <param name="properties"></param>
        //public override void ItemAdded(SPItemEventProperties properties)
        //{
        //    base.ItemAdded(properties);
        //}

        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            IncreaseItemOrderNo(properties);

            TrimTextFieldValue(properties, "Title");
            //TrimTextFieldValue(properties, "MenuKeywords");
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            IncreaseItemOrderNo(properties);

            TrimTextFieldValue(properties, "Title");
            //TrimTextFieldValue(properties, "MenuKeywords");
        }

        private void IncreaseItemOrderNo(SPItemEventProperties properties)
        {
            if (properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN] != null && !string.IsNullOrEmpty(properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN].ToString()))
            {
                int orderNo = 0;
                int.TryParse(properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN].ToString(), out orderNo);

                if (orderNo != 0)
                {
                    string strItemId = properties.ListItem == null ? "0" : properties.ListItemId.ToString();

                    string caml = string.Empty;
                    var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

                    expressionsAnd.Add(x => ((int)x[Constants.ORDER_NUMBER_COLUMN]) >= orderNo);
                    expressionsAnd.Add(x => (x["ID"]) != (DataTypes.Counter)strItemId);

                    //if (properties.AfterProperties[Constants.MENU_KEYWORDS_COLUMN] != null)
                    //{
                    //    expressionsAnd.Add(x => ((string)x[Constants.MENU_KEYWORDS_COLUMN]) == properties.AfterProperties[Constants.MENU_KEYWORDS_COLUMN].ToString());
                    //}

                    caml = Camlex.Query().WhereAll(expressionsAnd).OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Asc).ToString();

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = caml;

                    SPList currentList = properties.List;
                    SPListItemCollection items = currentList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        orderNo++;

                        using (DisableItemEvent scope = new DisableItemEvent())
                        {
                            item[Constants.ORDER_NUMBER_COLUMN] = orderNo;
                            item.SystemUpdate(false);
                        }
                    }
                }
            }
            else
            {
                properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN] = GetLatestItemOrderNo(properties) + 1;
            }
        }

        private int GetLatestItemOrderNo(SPItemEventProperties properties)
        {
            int orderNo = 0;

            string caml = string.Empty;
            var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

            //if (properties.AfterProperties[Constants.MENU_KEYWORDS_COLUMN] != null)
            //{
            //    expressionsAnd.Add(x => ((string)x[Constants.MENU_KEYWORDS_COLUMN]) == properties.AfterProperties[Constants.MENU_KEYWORDS_COLUMN].ToString());
            //}
            //else
            //{
            //    expressionsAnd.Add(x => (x[Constants.MENU_KEYWORDS_COLUMN]) == null);
            //}

            if (expressionsAnd.Count > 0)
                caml = Camlex.Query().WhereAll(expressionsAnd).OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Desc).ToString();
            else
                caml = Camlex.Query().OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Desc).ToString();

            SPQuery spQuery = new SPQuery();
            spQuery.Query = caml;

            spQuery.RowLimit = 1;

            SPList currentList = properties.List;
            SPListItemCollection items = currentList.GetItems(spQuery);

            if (items != null && items.Count > 0)
            {
                SPListItem item = items[0];

                if (item != null)
                {
                    orderNo = Convert.ToInt32(item[Constants.ORDER_NUMBER_COLUMN]);
                }
            }

            return orderNo;
        }

        private void TrimTextFieldValue(SPItemEventProperties properties, string fieldName)
        {
            if (properties.AfterProperties[fieldName] != null)
            {
                properties.AfterProperties[fieldName] = properties.AfterProperties[fieldName].ToString().Trim();
            }
        }
    }
}
