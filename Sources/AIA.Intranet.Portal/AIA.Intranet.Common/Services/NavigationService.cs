using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Common.Services
{
    public class NavigationService
    {
        private static SPList NavigationList = CCIUtility.GetListFromURL(Constants.NAVIGATION_LIST);


        public static Navigation GetNode(string path)
        {
            return null;
        }
        public static Navigation GetNodeByKey(string key)
        {
            return null;
        }

        public static SPFolder GetNodeByKey(string key, SPFolder folder)
        {
            if(folder == null) folder = NavigationList.RootFolder;

            if (folder.Item != null &&  folder.Item[IOfficeColumnId.NavigationKey] != null && folder.Item[IOfficeColumnId.NavigationKey].ToString() == key)
            {
                return folder;
            }
            if (folder.SubFolders.Count > 0)
                foreach (SPFolder item in folder.SubFolders)
                {
                    var p = GetNodeByKey(key, item);

                    if(item.SubFolders.Count>0 && p == null)
                        foreach (SPFolder item1 in item.SubFolders)
                        {
                             p = GetNodeByKey(key, item1);
                        }
                    if (p != null && p.Item!=null && p.Item[IOfficeColumnId.NavigationKey] != null && p.Item[IOfficeColumnId.NavigationKey].ToString() == key) 
                        return p;
                }
            else

                return folder;

            return null;
        }

        //public static void  AddSubNode(SPList list, string key, Navigation node)
        //{
        //    var folder = GetNodeByKey(key, list.RootFolder);
        //    if (folder != null)
        //    {
        //        AddItem(list, folder, new List<Navigation>() { node });
        //    }
        //}
        public static void AddItem(SPList navList, SPFolder root, params Navigation[] navigations)
        {
            AddItem(navList, root, navigations.Cast<Navigation>().ToList());
        }
        public static void AddItem(SPList navList, SPFolder root, List<Navigation> navigations)
        {
            foreach (var item in navigations)
            {
                try
                {
                    navList.ParentWeb.AllowUnsafeUpdates = true;
                    SPListItem newFolder = null;
                    //if (root.SubFolders[item.Name] != null && root.SubFolders[item.Name].Item != null) newFolder = root.SubFolders[item.Name].Item;

                    if (newFolder == null)
                    {
                        newFolder = navList.Items.Add(root.ServerRelativeUrl, SPFileSystemObjectType.Folder, item.Name);
                        newFolder.Update();
                    }

                    newFolder[IOfficeColumnId.NavigationUrl] = item.NavigationUrl;
                    newFolder[SPBuiltInFieldId.Title] = item.Name;
                    newFolder[IOfficeColumnId.STT] = item.Order;
                    newFolder[IOfficeColumnId.NavigationKey] = item.NavigationKey;
                    newFolder[SPBuiltInFieldId.ContentType] = "[I-Office] - Navigation Item";
                    newFolder.Update();

                    if (!string.IsNullOrEmpty(item.Groups))
                    {
                        var groups = item.Groups.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        SPRoleDefinition contributerDef = navList.ParentWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                        newFolder.BreakRoleInheritance(false);
                        newFolder.Update();
                        newFolder.SetPermissions(contributerDef.Name, groups.ToList());
                    }

                    try
                    {
                        if (item.Childrens != null)
                        {
                            AddItem(navList, newFolder.Folder, item.Childrens);
                        }
                    }
                    catch 
                    { 
                    }
                    navList.Update();
                }
                catch (Exception)
                {


                }
                finally { navList.ParentWeb.AllowUnsafeUpdates = true; }


            }
        }
    }
}
