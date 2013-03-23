using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPFieldLookupExtensions
    {
        /// <summary>
        /// Usage:

        ///    SPFieldLookup field = (SPFieldLookup)site.RootWeb.Fields[MyId];
        ///    field.UpdateLookupReferences(newWeb, newList)indexOfAttributeName ; 
        /// </summary>
        /// <param name="lookupField"></param>
        /// <param name="web"></param>
        /// <param name="list"></param>
        public static void UpdateLookupReferences(this SPFieldLookup lookupField, SPWeb web, SPList list)
        {
            try
            {
                web.AllowUnsafeUpdates = true;

                if (string.IsNullOrEmpty(lookupField.LookupList))
                {
                    lookupField.LookupWebId = web.ID;
                    lookupField.LookupList = list.ID.ToString();
                }
                else
                {
                    //lookupField.SchemaXml =
                    //    lookupField.SchemaXml
                    //        .ReplaceXmlAttributeValue("List", list.ID.ToString())
                    //        .ReplaceXmlAttributeValue("WebId", web.ID.ToString());

                    lookupField.SchemaXml =
                       lookupField.SchemaXml
                           .EnsureXmlAttribute("List", list.ID.ToString())
                           .EnsureXmlAttribute("WebId", web.ID.ToString());
                }

                lookupField.Update(true);
            }
            catch (Exception ex)
            {


            }
            finally
            {
                web.AllowUnsafeUpdates = false;

            }
        }
        public static void UpdateLookupReferences(this SPFieldLookup lookupField, string listUrl)
        {
            
            using (var web = lookupField.ParentList.ParentWeb.Site.OpenWeb(listUrl))
            {
                var list = web.GetList(listUrl);
                lookupField.UpdateLookupReferences(web, list);

            }
        }

        public static void UpdateLookupReferences(this SPFieldLookup lookupField, string listUrl, SPWeb web)
        {
            var list = CCIUtility.GetListFromURL(listUrl, web);
            
                lookupField.UpdateLookupReferences(web, list);

            
        }

    }
}
