using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class ConfigurationItem : BaseEntity
    {
        #region Properties
        public string Type { get; set; }
        public string HtmlValue { get; set; }
        public string Value { get; set; }

        #endregion


        public ConfigurationItem(DataRow item)
            : base(item)
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
         
        }

        public ConfigurationItem(SPListItem item)
            : base(item)
        {
            try
            {
            }
            catch (Exception)
            {
            }
           
        }

    }
}
