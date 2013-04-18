using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;

using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint.Navigation;


namespace Hypertek.IOffice.Infrastructure.Features.Hypertek.IOffice.Infrastructure.Site
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("93d1341d-f4c7-4a24-a7fe-79a801e4cec9")]
    public class HypertekIOfficeInfrastructureEventReceiver : SPFeatureReceiver
    {

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            SPWeb web = site.RootWeb;
            bool isAllowUnsafeUpdates = web.AllowUnsafeUpdates;

            try
            {
                web.AllowUnsafeUpdates = true;

                ConfigTopNavigationBar(web);

                AddBannerContentType(web);

                SetRootSitePermissions(web);

                ProvisionSubSitesStructure(web);

                ProvisionFeatures(web);

                ProvisionLeftMenu(web);
            }
            catch 
            { 
            }
            finally
            {
                web.AllowUnsafeUpdates = isAllowUnsafeUpdates;
            }
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            SPWeb web = site.RootWeb;

            bool isAllowUnsafeUpdates = web.AllowUnsafeUpdates;

            try
            {
                web.AllowUnsafeUpdates = true;

                RemoveMasterPage(web, "/_catalogs/masterpage/AIAPortal.master");
            }
            catch { }
            finally
            {
                web.AllowUnsafeUpdates = isAllowUnsafeUpdates;
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}


        #region Private Functions
        private void AddBannerContentType(SPWeb web)
        {
            try
            {
                var bannerList = Utility.GetListFromURL(Constants.BANNER_LIBRARY_URL, web);
                if (bannerList != null)
                {
                    SPContentTypeCollection spContentTypeCollection = web.ContentTypes;
                    SPContentType aiaBanerContentType = null;
                    foreach ( SPContentType temp in spContentTypeCollection)
                    {
                        if (temp.Name.Contains("Banner Content Type"))
                        {
                            aiaBanerContentType = temp;
                            break;
                        }
                    }

                    if (aiaBanerContentType != null && bannerList.ContentTypes[aiaBanerContentType.Name] == null)
                    {
                        bannerList.ContentTypes.Add(aiaBanerContentType);
                        System.Collections.Generic.List<SPContentType> result = new System.Collections.Generic.List<SPContentType>();
                        result.Add(aiaBanerContentType);
                        foreach (SPContentType ct in bannerList.ContentTypes)
                        {
                            if (ct.Name != aiaBanerContentType.Name)
                            {
                                bannerList.ContentTypes.Delete(ct.Id);
                            }
                        }
                    }
                    bannerList.Update();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Private Functions


        #region [Methods]
        private void ProvisionFeatures(SPWeb web)
        {
            try
            {
                web.Features.Add(new Guid(Constants.DATA_FEATURE_ID));
                web.Features.Add(new Guid(Constants.NEWS_FEATURE_ID));
            }
            catch
            {
            }
           
        }

        private void ProvisionSubSitesStructure(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.XMLCustomSettings.SiteStructures.xml");

                var subsites = SerializationHelper.DeserializeFromXml<WebDefinitionCollection>(xml);
                web.ProvisionWebStructure(subsites);

            }
            catch (Exception ex)
            {

            }
        }

        private void SetDefaultMasterPage(SPWeb web, string masterpageUrl)
        {
            Uri masterUri = new Uri(web.Url + masterpageUrl);

            web.MasterUrl = masterUri.AbsolutePath;
            web.CustomMasterUrl = masterUri.AbsolutePath;
            web.Update();
        }

        private void RemoveMasterPage(SPWeb web, string masterpageUrl)
        {
            SPFile  file = web.GetFile(web.ServerRelativeUrl.TrimEnd('/') + masterpageUrl);
            if (file.Exists)
                file.Delete();
            file.Update();
        }

        private void SetRootSitePermissions(SPWeb web)
        {
            try
            {
                string groupOwners = web.Title.Trim() + " Owners";
                web.CreateNewGroup(groupOwners, "Use this group to grant people full control permissions to the SharePoint site: " + web.Title.Trim(), SPRoleType.Administrator);

                string groupMembers = web.Title.Trim() + " Members";
                web.CreateNewGroup(groupMembers, "Use this group to grant people contribute permissions to the SharePoint site: " + web.Title.Trim(), SPRoleType.Contributor);

                string groupVisitors = web.Title.Trim() + " Visitors";
                web.CreateNewGroup(groupVisitors, "Use this group to grant people read permissions to the SharePoint site: " + web.Title.Trim(), SPRoleType.Reader);

                SPUser authenUsers = web.EnsureUser("NT AUTHORITY\\authenticated users");
                if (authenUsers != null)
                {
                    SPGroup spGrp = web.Groups[groupVisitors];
                    if (spGrp != null)
                    { 
                        spGrp.AddUser(authenUsers); 
                    }
                }
            }
            catch { }
        }

        private void ProvisionLeftMenu(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.XMLCustomSettings.DefaultLeftMenu.xml");

                var leftMenus = SerializationHelper.DeserializeFromXml<WebMenuDefinitionCollection>(xml);
                AddLeftMenu(web, leftMenus);

            }
            catch (Exception)
            {

            }
        }

        private void AddLeftMenu(SPWeb web, WebMenuDefinitionCollection leftMenus)
        {
            try
            {
                foreach (var menu in leftMenus)
                {
                    using (SPWeb spWeb = web.Site.OpenWeb(menu.Url))
                    {
                        var listLeftMenu = Utility.GetListFromURL(Constants.LEFT_MENU_LIST_URL, spWeb);
                        if (listLeftMenu == null) continue;
                        foreach (var leftMenu in menu.Features)
                        {
                            SPListItem item = listLeftMenu.Items.Add();
                            item["Title"] = leftMenu.Title;
                            item["URL"] = leftMenu.Url;
                            item["MenuKeywords"] = leftMenu.MenuKeywords;
                            item.SystemUpdate();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ConfigTopNavigationBar(SPWeb web)
        {
            SPNavigationNodeCollection topnav = web.Navigation.TopNavigationBar;

            if (topnav.Count == 1 && topnav[0].Title == "Home" && topnav[0].Url == web.ServerRelativeUrl)
            {
                topnav.Delete(topnav[0]);

                string linkTitle = web.Title;
                SPNavigationNode node = new SPNavigationNode(linkTitle, web.ServerRelativeUrl);
                node = topnav.AddAsFirst(node);
            }
        }
        #endregion [Methods]

    }
}
