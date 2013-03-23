using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Model;
using System.Linq;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Resources;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Workflow;
using AIA.Intranet.Infrastructure.Recievers;
namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("d99ce184-61ca-4a9e-ad0f-2bda1ee8c86f")]
    public class CategoryContentTypeEventReceiverEventReceiver : SPItemEventReceiver
    {
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            var ct = properties.ListItem.ContentType;
            var setting = ct.GetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);
            var listItem = properties.ListItem;
            if (setting == null || !setting.RunOnCreated || !setting.EnableCreateList) return;
            if (setting.EnableCreateList)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var url = listItem[setting.UrlFieldName].ToString();
                    var arr = url.Split(new char[] { ',' });
                    SPFieldUrlValue fieldValue = new SPFieldUrlValue(url);
                    fieldValue.Description = listItem.Title;
                    //fieldValue.Url = arr[1];

                    using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                    {
                        listItem[setting.UrlFieldName] = fieldValue;
                        listItem.SystemUpdate();
                    }
                });
            }
        }

        public override void ItemUpdating(SPItemEventProperties properties)
        {
            var ct = properties.ListItem.ContentType;
            var setting = ct.GetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);
            var listItem = properties.ListItem;
            if (setting == null || !setting.RunOnCreated || !setting.EnableCreateList) return;
            if (setting.EnableCreateList)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var url = listItem[setting.UrlFieldName].ToString();

                    //sync SPList title
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.OpenWeb(properties.Web.ID))
                        {
                            try
                            {
                                web.AllowUnsafeUpdates = true;
                                //update List title
                                SPList destList = web.Lists.TryGetList(listItem.Title);
                                if (destList != null)
                                {
                                    destList.Title = properties.AfterProperties["Title"].ToString();
                                    destList.Update();
                                }


                                SPWeb rootWeb = site.RootWeb;

                                //update Navigation title
                                SPList navList = rootWeb.GetList(rootWeb.ServerRelativeUrl.TrimEnd('/') + Constants.NAVIGATION_LIST);

                                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                                expressions.Add(x => ((string)x[SPBuiltInFieldId.Title]) == listItem.Title);
                                string camlNav = Camlex.Query().WhereAll(expressions).ToString();

                                SPQuery queryNav = new SPQuery();
                                queryNav.Query = camlNav;
                                queryNav.ViewAttributes = "Scope=\"RecursiveAll\"";

                                SPListItemCollection navItems = navList.GetItems(queryNav);
                                if (navItems != null && navItems.Count > 0)
                                {
                                    foreach (SPListItem item in navItems)
                                    {
                                        if (item.Folder.ParentFolder.Name.ToLower() == web.Title.ToLower())
                                        {
                                            item[SPBuiltInFieldId.Title] = properties.AfterProperties["Title"].ToString();
                                            item[SPBuiltInFieldId.FileLeafRef] = properties.AfterProperties["Title"].ToString();
                                            item.Update();
                                        }
                                    }
                                }

                            }
                            catch { }
                            finally {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });


            }
        }

        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);

            var ct = properties.ListItem.ContentType;
            var setting = ct.GetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);
            var listItem = properties.ListItem;
            if (setting == null || !setting.RunOnCreated || !setting.EnableCreateList) return;
            if (setting.EnableCreateList)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    var url = listItem[setting.UrlFieldName].ToString();

                    //sync SPList title
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.OpenWeb(properties.Web.ID))
                        {
                            try
                            {
                                web.AllowUnsafeUpdates = true;
                                //delete List
                                SPList destList = web.Lists.TryGetList(listItem.Title);
                                if (destList != null)
                                {
                                    destList.Delete();
                                }

                                SPWeb rootWeb = site.RootWeb;

                                //delete Navigation
                                SPList navList = rootWeb.GetList(rootWeb.ServerRelativeUrl.TrimEnd('/') + Constants.NAVIGATION_LIST);

                                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                                expressions.Add(x => ((string)x[SPBuiltInFieldId.Title]) == listItem.Title);
                                string camlNav = Camlex.Query().WhereAll(expressions).ToString();

                                SPQuery queryNav = new SPQuery();
                                queryNav.Query = camlNav;
                                queryNav.ViewAttributes = "Scope=\"RecursiveAll\"";

                                SPListItemCollection navItems = navList.GetItems(queryNav);
                                if (navItems != null && navItems.Count > 0)
                                {
                                    foreach (SPListItem item in navItems)
                                    {
                                        if (item.Folder.ParentFolder.Name.ToLower() == web.Title.ToLower())
                                        {
                                            item.Delete();
                                        }
                                    }
                                }
                            }
                            catch { }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });


            }
        }

        /// <summary>
        /// Asynchronous After event that occurs after a new item has been added to its containing object.
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            var ct = properties.ListItem.ContentType;
            var setting = ct.GetCustomSettings<AutoCreationSettings>(IOfficeFeatures.Infrastructure);

            if (setting == null || !setting.RunOnCreated || !setting.EnableCreateList) return;
            string listNewCategoryUrl = string.Empty;
            if (setting.EnableCreateList)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.OpenWeb(properties.Web.ID))
                        {
                            CreateList(properties.ListItem, web, setting.ListDefinition, setting.UrlFieldName, out listNewCategoryUrl);
                        }
                    }
                });
            }

            //add Navigation
            if (setting.EnableNavigationUpdate && !string.IsNullOrEmpty(listNewCategoryUrl))
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        SPWeb rootWeb = site.RootWeb;
                        CreateNavigation(rootWeb, listNewCategoryUrl, properties.ListItem, setting.NavigationUpdate);
                    }
                });
            }

            base.ItemAdded(properties);
        }

        private void CreateList(SPListItem listItem, SPWeb web, ListIntanceDefinition definition, string UrlFieldName, out string listUrl)
        {
            listUrl = string.Empty;
            try
            {
                var template = web.ListTemplates.Cast<SPListTemplate>().Where(p => p.Name == definition.TemplateName).FirstOrDefault();
                if (template == null) return;
                string title = listItem.GetFormulaValue(definition.Title);
                string url = listItem.GetFormulaValue(definition.Url).Simplyfied();
                if (template.CategoryType == SPListCategoryType.CustomLists)
                    url = "Lists/" + url;

                var listId = web.Lists.Add(title, string.Empty, url, template.FeatureId.ToString(), template.Type_Client, "100", SPListTemplate.QuickLaunchOptions.Default);
               // Guid listId = web.Lists.Add(item.Title, item.Description, item.Url, template.FeatureId.ToString(), item.TemplateId, "100");

                SPList list = web.Lists[listId];
                list.ContentTypesEnabled = true;
                list.EnableVersioning = true;
                list.EnableModeration = true; //enable Content Approval
                foreach (var item in definition.ContentTypes)
                {
                    list.EnsureContentTypeInList(item);
                }


                

                 var urlValue = new SPFieldUrlValue();
                urlValue.Description = title;
                urlValue.Url = web.Url + "/" + url;

                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    listItem[UrlFieldName] = urlValue;
                    listItem.Update();
                }


                listUrl = urlValue.Url;
                list.Update();
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.Infrastructure);
                //throw;
            }
        }

        private void CreateNavigation(SPWeb rootWeb, string listNewCategoryUrl, SPListItem listItem, NavigationUpdateProperties navigationUpdate)
        {
            SPList listNavigation = rootWeb.GetList(Constants.NAVIGATION_LIST);
            //SPFolder folder = NavigationService.GetNodeByKey("NEWS", listNavigation.RootFolder);
            SPFolder folder = NavigationService.GetNodeByKey(navigationUpdate.Key, listNavigation.RootFolder);
            int itemCount = 0;
            if (folder.SubFolders != null)
            {
                itemCount += folder.SubFolders.Count;
            }
            string title = listItem.GetFormulaValue(navigationUpdate.Title);

            Navigation naviga = new Navigation();
            naviga.Name = title;
            naviga.NavigationUrl = listNewCategoryUrl;
            naviga.Order = itemCount;

            NavigationService.AddItem(listNavigation, folder, naviga);
        }

        #region [RoomBookingSystem methods]
        private void SetRoomBookingListPermission(SPWeb web, SPList list, params string[] groups)
        {
            try
            {
                web.AllowUnsafeUpdates = true;

                if (!list.HasUniqueRoleAssignments)
                    list.BreakRoleInheritance(false);

                List<Assignement> assignments = new List<Assignement>();

                foreach (string group in groups)
                {
                    assignments.Add(new Assignement() { Name = group, RoleDefinitions = new List<SPRoleType>() { SPRoleType.Contributor } });
                }

                list.UpdatePermissions(assignments, false, web);
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("CategoryContentTypeEventReceiverEventReceiver SetRoomBookingListPermission", ex.Message);
            }
            finally
            {
                web.AllowUnsafeUpdates = true;
            }
        }
        #endregion [RoomBookingSystem methods]

        #region [Common methods]
        private void CreateApprovalWorkflow(SPList list, string workflowTemplateName, string workflowName, string fileSettings)
        {
            string association = string.Empty;

            if (!string.IsNullOrEmpty(fileSettings))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile(fileSettings);

                association = SerializationHelper.SerializeToXml(SerializationHelper.DeserializeFromXml<ApprovalWFAssociationData>(xml));
            }

            list.AssociateWorkflow(workflowTemplateName, workflowName, association);
        }

        #endregion [Common methods]
    }
}
