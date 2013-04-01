using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;

namespace AIA.Intranet.Infrastructure.Features.AIA.UpdateWebConfig
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("b8869cd1-3829-496b-9cd0-38803dd21c7e")]
    public class AIAEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        private readonly List<SPWebConfigModification> mListModifications = new List<SPWebConfigModification>();
        protected string OwnerModif
        {
            get { return "AIA.Intranet.WebConfig"; }
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            const bool removeModification = false;
            AddOrRemoveHandler(properties, removeModification);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            const bool removeModification = true;
            AddOrRemoveHandler(properties, removeModification);
        }

        protected void AddOrRemoveHandler(SPFeatureReceiverProperties properties, bool removeModification)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            SPWebApplication app = properties.Feature.Parent as SPWebApplication;

            if (app == null)
            {
                SPSite site = properties.Feature.Parent as SPSite;

                if (site == null)
                {
                    SPWeb web = properties.Feature.Parent as SPWeb;
                    if (web != null)
                    {
                        app = web.Site.WebApplication;
                    }
                }
                else
                {
                    app = site.WebApplication;
                }
            }

            if (app == null)
            {
                throw new ArgumentOutOfRangeException(properties.Feature.Parent.ToString());
            }
            if (!removeModification)
            {
                AddConfigurationToWebConfig(app);
            }
            else
            {
                RemoveWebConfigEntries(app, OwnerModif);
            }
            app.Update();
        }

        protected void AddConfigurationToWebConfig(SPWebApplication app)
        {
            /*
            //Sample
            AddNodeValue("SafeControl[@id='XXXXXXX']",
            "configuration/SharePoint/SafeControls",
            @"<SafeControl YYYYYYYYYYYY id='XXXXXXXXXXXX'/>");
            */

            AddNodeValue("add[@name='AIAPortalHttpModule']",
                        "configuration/system.webServer/modules",
                        "<add name=\"AIAPortalHttpModule\" type=\"AIA.Intranet.Infrastructure.AIAPortalHttpModule,  AIA.Intranet.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0b6a88a58a49868d\"/>");

            SaveWebConfig(app);
        }

        protected void AddNodeValue(string name, string xpath, string scriptResource)
        {
            mListModifications.Add(GetSPWebModifChild(name, xpath, scriptResource));
        }

        //Section cannot be removed. So use AddNodeValue() instead.
        protected void AddSection(string name, string xpath)
        {
            mListModifications.Add(GetSPWebModifSection(name, xpath));
        }

        protected void ModifyAttribute(string name, string xpath, string value)
        {
            //Sample: 
            /*
                 GetSPWebModifAttribut(
                    "mode",
                    "configuration/system.web/customErrors",
                    @"Off"
                 )
             */
            mListModifications.Add(GetSPWebModifAttribut(name, xpath, value));
        }

        protected SPWebConfigModification GetSPWebModifAttribut(string name, string xpath, string value)
        {
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = OwnerModif;
            modification.Sequence = 0;
            modification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute;
            modification.Value = value;
            return modification;
        }

        protected SPWebConfigModification GetSPWebModifChild(string name, string xpath, string scriptResource)
        {
            // name: Make sure that the name is a unique XPath selector for the element we are adding. This name is used for removing the element.
            // xpath: The XPath to the location of the parent node in web.config
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            // Owner: The owner of the web.config modification, useful for removing a group of modifications
            modification.Owner = OwnerModif;
            // Sequence: is important if there are multiple equal nodes that can't be identified with an XPath expression
            modification.Sequence = 0;
            modification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            // Value: The XML to insert as child node, make sure that used names match the Name selector
            modification.Value = scriptResource;
            return modification;
        }

        protected SPWebConfigModification GetSPWebModifSection(string name, string xpath)
        {
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = OwnerModif;
            modification.Sequence = 0;
            modification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            return modification;
        }

        protected void RemoveWebConfigEntries(SPWebApplication oWebApp, string owner)
        {
            Collection<SPWebConfigModification> webConfigModifications = oWebApp.WebConfigModifications;
            int num = webConfigModifications.Count;
            for (int i = num - 1; i >= 0; i--)
            {
                SPWebConfigModification modification = webConfigModifications[i];
                if (modification.Owner == owner)
                {
                    webConfigModifications.Remove(modification);
                }
            }
            if (num > webConfigModifications.Count)
            {
                oWebApp.Update();
                SPFarm.Local.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            }
        }

        protected void SaveWebConfig(SPWebApplication application)
        {
            foreach (SPWebConfigModification m in mListModifications)
            {
                application.WebConfigModifications.Add(m);
            }

            mListModifications.Clear();

            SPFarm.Local.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
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
    }
}
