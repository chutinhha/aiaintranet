using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class LinkItem: BaseEntity
    {
        public LinkItem(SPListItem item ) : base (item){

            SPFieldUrlValue fieldValue = new SPFieldUrlValue(item[SPBuiltInFieldId.URL].ToString());

            string []arr = fieldValue.Url.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 2)
            {
                fieldValue.Url = arr[0];
                fieldValue.Description = arr[1];
                item[SPBuiltInFieldId.URL] = fieldValue;
                //item.Update();
            }
            Url = fieldValue.Url;
            Description = fieldValue.Description;
        }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }

    }
}
