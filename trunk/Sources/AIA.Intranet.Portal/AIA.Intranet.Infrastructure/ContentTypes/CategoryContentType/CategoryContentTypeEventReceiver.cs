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
using AIA.Intranet.Infrastructure.Receivers;
namespace AIA.Intranet.Infrastructure.ContentTypes
{
    [Guid("d99ce184-61ca-4a9e-ad0f-2bda1ee8c86f")]
    public class CategoryContentTypeEventReceiverEventReceiver : SPItemEventReceiver
    {
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            var ct = properties.ListItem.ContentType;
            var setting = ct.GetCustomSettings<AutoCreationSettings>(AIAPortalFeatures.Infrastructure);
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
            var setting = ct.GetCustomSettings<AutoCreationSettings>(AIAPortalFeatures.Infrastructure);
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
            var setting = ct.GetCustomSettings<AutoCreationSettings>(AIAPortalFeatures.Infrastructure);
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
            var setting = ct.GetCustomSettings<AutoCreationSettings>(AIAPortalFeatures.Infrastructure);

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
                Utility.LogError(ex.Message + ex.StackTrace, AIAPortalFeatures.Infrastructure);
                //throw;
            }
        }
    }
}
