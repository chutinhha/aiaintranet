using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities;

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
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            IncreaseItemOrderNo(properties);
        }

        private void IncreaseItemOrderNo(SPItemEventProperties properties)
        {
            if (properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN] != null)
            {
                int orderNo = 0;
                int.TryParse(properties.AfterProperties[Constants.ORDER_NUMBER_COLUMN].ToString(), out orderNo);

                if (orderNo != 0)
                {

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = String.Format(@"<Where>
                                                          <And>
                                                             <Geq>
                                                                <FieldRef Name='{0}' />
                                                                <Value Type='Number'>{1}</Value>
                                                             </Geq>
                                                            <Neq>
                                                                <FieldRef Name='{2}' />
                                                                <Value Type='Counter'>{3}</Value>
                                                            </Neq>
                                                          </And>
                                                    </Where>
                                                   <OrderBy>
                                                      <FieldRef Name='{4}' Ascending='True' />
                                                   </OrderBy>", Constants.ORDER_NUMBER_COLUMN, orderNo.ToString()
                                                              , "ID", properties.ListItem == null ? "0" : properties.ListItemId.ToString()
                                                              , Constants.ORDER_NUMBER_COLUMN /*OrderBy*/);

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
        }
    }
}
