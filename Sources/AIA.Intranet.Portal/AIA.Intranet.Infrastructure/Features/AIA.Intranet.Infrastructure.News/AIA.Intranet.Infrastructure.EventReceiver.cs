using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Infrastructure;
using System.Collections.Generic;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.Features.AIA.Intranet.Infrastructure.News
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("009541f9-7808-49c9-990e-59a5d6e3e96f")]
    public class AIAIntranetInfrastructureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            UpdateNewsCategoroySettings(web);
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

        #region Private Functions
        private void UpdateNewsCategoroySettings(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.NewsCustomSettings.xml");

                var settings = SerializationHelper.DeserializeFromXml<List<CustomSettingDefinition>>(xml);

                web.UpdateCustomSetting(settings);

            }
            catch (Exception)
            {

            }
        }
        #endregion Private Functions
    }
}
