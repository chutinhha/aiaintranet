using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex.Interfaces;
using System.Reflection;

namespace AIA.Intranet.Common.Utilities
{
    public class CAMLListQuery<T> where T : IBaseEntity
    {
        protected SPList Source { get; set; }

        public CAMLListQuery(SPList sourceList)
        {
            Source = sourceList;
        }

        public CAMLListQuery(string listUrl)
        {
            Source = CCIUtility.GetListFromURL(listUrl);
        }

        public SPListItemCollection GetItems(string caml)
        {
            SPQuery query = new SPQuery();
            query.Query = caml;

            var data = Source.GetItems(query);
            return data;
        }
        public SPListItem GetItem(string caml)
        {
            SPQuery query = new SPQuery();
            query.Query = caml;

            var data = Source.GetItems(query);
            if (data != null && data.Count > 0) return data[0];
            return null;

        }

        public List<T> ExecuteListQuery(string CAML)
        {
            return ExecuteListQuery(CAML, 0);
        }

        public List<T> ExecuteListQuery(string CAML, int rowLimit)
        {
            SPListItemCollection dump = null;
            return ExecuteListQuery(CAML, rowLimit, string.Empty, out dump);
        }

        public List<T> ExecuteListQuery(string CAML, int rowLimit, string pagingInfo, out SPListItemCollection collection)
        {
            collection = null;

            List<T> results = new List<T>();
            SPQuery query = new SPQuery();
            query.Query = CAML;

            if (rowLimit != 0)
            {
                query.RowLimit = (uint)rowLimit;
            }

            //use for paging
            if (!string.IsNullOrEmpty(pagingInfo))
            {
                SPListItemCollectionPosition position = new SPListItemCollectionPosition(pagingInfo);
                query.ListItemCollectionPosition = position;
            }

            if (Source == null) return results;
            var data = Source.GetItems(query);

            //use for paging
            collection = data;


            foreach (SPListItem item in data)
            {
                var resolvedItem = CreateInstance(item);
                if (resolvedItem != null)
                    results.Add(resolvedItem);

            }
            return results;
        }

        public T ExecuteSingleQuery(string CAML)
        {
            return ExecuteListQuery(CAML).FirstOrDefault();
        }

        public T GetItemById(int id)
        {
            SPListItem item = null;

            try
            {
                 item = Source.GetItemById(id);
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex);
            }
            
            return CreateInstance(item);
        }

        public T CreateInstance(SPListItem item)
        {
            if(item == null) return default(T);

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
       
    

