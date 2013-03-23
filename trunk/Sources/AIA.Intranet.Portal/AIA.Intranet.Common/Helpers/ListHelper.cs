using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;


namespace AIA.Intranet.Infrastructure.Utilities
{
    public class ListHelper
    {
        public static List<string> GetTextFields(string listUrl)
        {
            SPSite site = SPContext.Current.Site;
            SPWeb rootWeb = site.RootWeb;
            List<string> fields = new List<string>();
            try 
	        {
                SPList oUList = rootWeb.GetList(listUrl);
                var q = from field in oUList.Fields.Cast<SPField>().ToList()
                        where field.Type == SPFieldType.Text && !field.Hidden
                        select field.Title;
                return q.Distinct().ToList<string>();
	        }
	        catch (Exception ex)
	        {
                System.Diagnostics.Debug.Write(ex);
	        }
            return new List<string>();
        }

        public static List<string> GetAllValueInList(string listUrl, string fieldName)
        {
            SPSite site = SPContext.Current.Site;
            SPWeb rootWeb = site.RootWeb;
            List<string> fields = new List<string>();
            try 
	        {
                SPList oUList = rootWeb.GetList(listUrl);
                SPListItemCollection items = oUList.Items;

                var q = from item in oUList.Items.Cast<SPListItem>().ToList()
                        where item[fieldName] != null
                        select item[fieldName].ToString();
                return q.ToList<string>();
	        }
	        catch (Exception ex)
	        {
                System.Diagnostics.Debug.Write(ex);
	        }
            return new List<string>();
        }
    }
}
