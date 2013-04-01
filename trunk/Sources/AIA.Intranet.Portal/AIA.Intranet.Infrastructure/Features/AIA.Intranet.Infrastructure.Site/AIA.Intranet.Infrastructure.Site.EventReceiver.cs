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
        string customizedMasterUrl = "/_catalogs/masterpage/AIAPortal.master";


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
                //SetDefaultCustomMasterPage(web, "/_catalogs/masterpage/AIAPortal.master");
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

                //SetDefaultCustomMasterPage(web, "/_catalogs/masterpage/v4.master");
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
                    bannerList.ContentTypes.Add(web.AvailableContentTypes["AIA] - Banner Content Type"]);
                    foreach (SPContentType item in bannerList.ContentTypes)
                    {
                        if (item.Parent.Id == SPBuiltInContentTypeId.Picture)
                        {
                            bannerList.ContentTypes.Delete(item.Id);
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

        private void SetDefaultCustomMasterPage(SPWeb web, string masterpageUrl)
        {
            Uri masterUri = new Uri(web.Url + masterpageUrl);

            web.MasterUrl = masterUri.AbsolutePath;
            web.CustomMasterUrl = masterUri.AbsolutePath;
            web.Update();
        }
        #endregion [Methods]

    }
}