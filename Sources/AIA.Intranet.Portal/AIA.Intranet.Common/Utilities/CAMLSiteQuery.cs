using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex.Interfaces;
using System.Reflection;
using System.Data;

namespace AIA.Intranet.Common.Utilities
{
    public class CAMLSiteQuery<T> where T : IBaseEntity
    {
        protected SPWeb Source { get; set; }
        public string TemplateId { get; set; }
        public CAMLSiteQuery(SPWeb sourceList)
        {
            Source = sourceList;
        }
        public CAMLSiteQuery(string templateId, SPWeb web)
        {
            TemplateId = templateId;
            Source = web;
        }

       

        public DataTable GetItems(string caml,  int rowLimit)
        {

            // Fetch using SPSiteDataQuery
            SPSiteDataQuery query = new SPSiteDataQuery();
            query.Lists = string.Format("<Lists ServerTemplate=\"{0}\" />", this.TemplateId);
            query.ViewFields = GetViewFields();
            query.Webs = "<Webs Scope=\"SiteCollection\" />";
            query.Query = caml;

            if (rowLimit != 0)
            {
                query.RowLimit = (uint)rowLimit;
            }
            
            var data = Source.Site.RootWeb.GetSiteData(query);

            return data;
        }

        private string GetViewFields()
        {
            StringBuilder sb = new StringBuilder();
             Type type = typeof(T);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var item in properties)
            {
                var attrs = item.GetCustomAttributes(typeof(FieldAttribute), true);
                string fieldName = item.Name;
                string fieldType = "Text";

                if (attrs != null && attrs.Length > 0)
                {
                    FieldAttribute fieldAttr = attrs[0] as FieldAttribute;
                    fieldType = fieldAttr.Type;
                    if (fieldAttr.Ignore) continue;
                    fieldName = fieldAttr.FieldName;
                }


                //sb.AppendFormat("<FieldRef Name=\"{0}\" Nullable=\"TRUE\" Type=\"{1}\"/>", fieldName, fieldType);
                
                sb.AppendFormat("<FieldRef Name=\"{0}\" Nullable=\"TRUE\" />", fieldName);
                
            }

            sb.AppendFormat("<FieldRef Name=\"{0}\" Nullable=\"TRUE\" />", "FileRef");
            sb.AppendFormat("<FieldRef Name=\"{0}\" Nullable=\"TRUE\" />", "EncodedAbsUrl");
            return sb.ToString();

        }
       
        
        public List<T> ExecuteListQuery(string CAML, int rowCount)
        {


            var data = GetItems(CAML, rowCount);

            List<T> results = new List<T>();
            foreach (DataRow item in data.Rows)
            {
                
                var resolvedItem = CreateInstance(item);
                if (resolvedItem != null)
                    results.Add(resolvedItem);

            }
            return results;
        }

        private T CreateInstance(DataRow item)
        {
            Type classType = typeof(T);
            T classInstance = default(T);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { item.GetType() });
            if (classConstructor != null)
            {
                try
                {
                    classInstance = (T)classConstructor.Invoke(new object[] { item });
                    return classInstance;
                }
                catch { }
                finally { }
            }
            return classInstance;
        }

        
        
    }
}



