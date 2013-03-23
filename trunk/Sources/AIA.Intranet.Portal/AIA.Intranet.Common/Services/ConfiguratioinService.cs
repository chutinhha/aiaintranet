using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Common.Services
{
    public class ConfiguratioinService
    {
        public static ConfigurationItem GetItemByKey(string key, SPWeb web)
        {
            SPList list = CCIUtility.GetListFromURL(Constants.CONFIG_LIST_URL, web);
            if (list == null) return null;
            CAMLListQuery<ConfigurationItem> query = new CAMLListQuery<ConfigurationItem>(list);

            string caml = Camlex.Query().Where(x => (string)x[SPBuiltInFieldId.Title] == key).ToString();
            return query.ExecuteSingleQuery(caml);

        }

        public static ConfigurationItem GetItemByKey(string key)
        {
            return GetItemByKey(key, null);
        }
        public static List<ConfigurationItem> GetAllItems(SPWeb web)
        {
            SPList list = CCIUtility.GetListFromURL(Constants.CONFIG_LIST_URL, web);
            if (list == null) return null;
            CAMLListQuery<ConfigurationItem> query = new CAMLListQuery<ConfigurationItem>(list);

            
            return query.ExecuteListQuery(string.Empty);
        }

        //public static List<ConfigurationItem> GetAllMaillist(SPWeb web)
        //{
        //    return GetAllItems(web, Constants.MAIL_LIST_TYPE);
        //}
        public static List<ConfigurationItem> GetAllItems(SPWeb web, string type)
        {
            List<ConfigurationItem> results = new List<ConfigurationItem>();
            SPList list = CCIUtility.GetListFromURL(Constants.CONFIG_LIST_URL, web);
            if (list != null)
            {
                CAMLListQuery<ConfigurationItem> query = new CAMLListQuery<ConfigurationItem>(list);
                string caml = Camlex.Query().Where(x => x["Type"] == (DataTypes.Choice)type).ToString();

                results =  query.ExecuteListQuery(caml);
            }
            return results;
        }

        public static List<ConfigurationItem> GetAllItems()
        {
            return GetAllItems(null);
        }
    }
}
