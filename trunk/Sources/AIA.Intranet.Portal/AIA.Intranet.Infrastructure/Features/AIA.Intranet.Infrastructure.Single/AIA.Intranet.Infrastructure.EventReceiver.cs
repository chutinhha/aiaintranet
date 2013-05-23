using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using AIA.Intranet.Model;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model.Entities;
namespace AIA.Intranet.Infrastructure.Features.AIA.Intranet.Infrastructure.Single
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("44059d77-187b-46d4-82e7-6774f5911939")]
    public class AIAIntranetInfrastructureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            var folder = web.RootFolder;
            folder.WelcomePage = Constants.NEWS_HOME_PAGE.TrimStart('/');
            folder.Update();
            ProvisionFeatures(web);
            ProvisionWebpart(web, "AIA.Intranet.Infrastructure.XMLCustomSettings.SingleWebParts.xml");
            ProvisionLeftMenu(web);
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
        #region Functions
        private void ProvisionWebpart(SPWeb web, string xmlFile)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile(xmlFile);

                var webpartPage = SerializationHelper.DeserializeFromXml<WebpartPageDefinitionCollection>(xml);

                WebPartHelper.ProvisionWebpart(web, webpartPage);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }
        }

        private void ProvisionFeatures(SPWeb web)
        {
            try
            {
                web.Features.Add(new Guid(Constants.NEWS_FEATURE_ID));
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }

        }

        private void ProvisionLeftMenu(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.XMLCustomSettings.LeftMenu.xml");

                var leftMenus = SerializationHelper.DeserializeFromXml<WebMenuDefinitionCollection>(xml);
                AddLeftMenu(web, leftMenus);

            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }
        }

        private void AddLeftMenu(SPWeb web, WebMenuDefinitionCollection leftMenus)
        {
            try
            {
                foreach (var menu in leftMenus)
                {
                    var listLeftMenu = Utility.GetListFromURL(Constants.LEFT_MENU_LIST_URL, web);
                    if (listLeftMenu == null) continue;
                    foreach (var leftMenu in menu.Features)
                    {
                        SPListItem item = listLeftMenu.Items.Add();
                        item["Title"] = leftMenu.Title;
                        item["URL"] = web.ServerRelativeUrl.TrimEnd('/') + leftMenu.Url;
                        //item["MenuKeywords"] = leftMenu.MenuKeywords;
                        item.SystemUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }
        }

        #endregion Functions
    }
}
