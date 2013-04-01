using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;


namespace AIA.Intranet.Common.Core
{
    [PermissionSet(SecurityAction.Demand)]
    public abstract class WebConfigModifier : SPFeatureReceiver
    {
        // Fields
        private List<SPWebConfigModification> mListModifications = new List<SPWebConfigModification>();

        // Methods
        protected WebConfigModifier()
        {
        }

        protected abstract void AddConfigurationToWebConfig(SPWebApplication app);

        protected void AddNodeValue(string name, string xpath, string scriptResource, SPWebConfigModification.SPWebConfigModificationType type, int sequence)
        {
            this.mListModifications.Add(this.GetSPWebModifChild(name, xpath, scriptResource,type, sequence));
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
               SPSite site  = properties.Feature.Parent as SPSite;

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
            SPSecurity.RunWithElevatedPrivileges(delegate {

                string webUrl = app.GetResponseUri(SPUrlZone.Default).AbsoluteUri;

                SPWebApplication securedWebbApp = SPWebApplication.Lookup(new Uri(webUrl));
                if (!removeModification)
                {
                    this.AddConfigurationToWebConfig(securedWebbApp);
                }
                else
                {
                    this.RemoveWebConfigEntries(securedWebbApp, this.OwnerModify);
                }
                securedWebbApp.Update();

            });
        }

        protected void AddSection(string name, string xpath)
        {
            this.mListModifications.Add(this.GetSPWebModifSection(name, xpath));
        }


        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            bool removeModification = false;
            this.AddOrRemoveHandler(properties, removeModification);
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            bool removeModification = true;
            this.AddOrRemoveHandler(properties, removeModification);
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        protected SPWebConfigModification GetSPWebModifAttribut(string name, string xpath, string value)
        {
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = this.OwnerModify;
            modification.Sequence = 0;
            modification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute;
            modification.Value = value;
            return modification;
        }
       
        protected SPWebConfigModification GetSPWebModifChild(string name, string xpath, string scriptResource,SPWebConfigModification.SPWebConfigModificationType type,int sequence )
        {
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = this.OwnerModify;
            modification.Sequence = (uint)sequence;
            modification.Type = type;
            modification.Value = scriptResource;
            return modification;
        }
        protected SPWebConfigModification GetSPWebModifSection(string name, string xpath)
        {
            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = this.OwnerModify;
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
                if (string.Compare(modification.Owner, owner, true) == 0)
                {
                    webConfigModifications.Remove(modification);
                }
            }
            if (num > webConfigModifications.Count)
            {
                updateWebConfigPhysicalFile(oWebApp);
            }
        }

        protected void SaveWebConfig(SPWebApplication application)
        {

            foreach (SPWebConfigModification m in this.mListModifications)
            {
                application.WebConfigModifications.Add(m);
            }

            this.mListModifications.Clear();
            updateWebConfigPhysicalFile(application);
  
        }
        protected void EnableViewState(SPWebApplication application)
        {

        }
        private static void updateWebConfigPhysicalFile(SPWebApplication app)
        {
            app.WebService.ApplyWebConfigModifications();
            return;

            if (SPWebService.ContentService.RemoteAdministratorAccessDenied)
            {
                SPWebService.ContentService.RemoteAdministratorAccessDenied = false;
                SPWebService.ContentService.Update();

                
                if (SPFarm.Local.TimerService.Instances.Count == 0)
                {
                    SPFarm.Local.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                }

                SPWebService.ContentService.RemoteAdministratorAccessDenied = true;
                SPWebService.ContentService.Update();
            }
            else
            {
               // if (SPFarm.Local.TimerService.Instances.Count == 0)
                {
                    app.WebService.ApplyWebConfigModifications();
                }
            }
        }

        // Properties
        protected abstract string OwnerModify { get; }
    }

}
