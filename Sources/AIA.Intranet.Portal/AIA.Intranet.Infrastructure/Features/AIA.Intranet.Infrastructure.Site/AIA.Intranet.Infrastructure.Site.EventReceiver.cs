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

                AddBannerContentType(web);

                ProvisionSubSitesStructure(web);
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
                var bannerList = CCIUtility.GetListFromURL(Constants.BANNER_LIBRARY_URL, web);
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
        private void ProvisionSubSitesStructure(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.XMLCustomSettings.SiteStructures.xml");

                var subsites = SerializationHelper.DeserializeFromXml<WebDefinitionCollection>(xml);
                web.ProvisionWebStructure(subsites);

            }
            catch (Exception)
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
        #endregion [Methods]

    }
}
