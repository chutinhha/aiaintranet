using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Resources;
using System.Globalization;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Infrastructure.Features.AIA.Intranet.Infrastructure.Data
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("cdf8a458-365c-4011-b31c-40771b64d22e")]
    public class HypertekIOfficeInfrastructureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            var folder = web.RootFolder;
            folder.WelcomePage = Constants.NEWS_HOME_PAGE.TrimStart('/');
            folder.Update();
            ProvisionWebpart(web, "AIA.Intranet.Infrastructure.Webparts.xml");
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
                
                throw;
            }
        }
        #endregion Functions
    }
}
