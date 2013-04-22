using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using System.Linq.Expressions;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.ContentTypes
{
    public class NewsItemContentTypeEventReciever : SPItemEventReceiver
    {
        public override void ItemAdding(SPItemEventProperties properties)
        {
            IncreaseItemOrderNo(properties, false);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            IncreaseItemOrderNo(properties, true);
        }

        private void IncreaseItemOrderNo(SPItemEventProperties properties, bool isUpdate)
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
                if (!isUpdate)
                    properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN] = GetLatestItemOrderNo(properties) + 1;
            }
        }

        private int GetLatestItemOrderNo(SPItemEventProperties properties)
        {
            int orderNo = 0;

            string caml = string.Empty;
            var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

            
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
    }
}
