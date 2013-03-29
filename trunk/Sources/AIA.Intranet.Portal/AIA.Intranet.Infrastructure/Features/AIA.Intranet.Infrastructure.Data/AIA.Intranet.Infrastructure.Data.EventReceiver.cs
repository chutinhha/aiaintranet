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
            //AddDepartmentItem(web);
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

        private void AddDepartmentItem(SPWeb web)
        {
            try
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    string siteName = CommonResources.ManagementGroup;
                    SPList departmentList = CCIUtility.GetListFromURL(Constants.DEPARTMENT_LIST_URL, web);

                    SPQuery spQuery = new SPQuery();
                    string query = Camlex.Query()
                        .Where(x => ((string)x["Link"]).Contains(siteName)).ToString();



                    SPListItemCollection splistItemCollection = departmentList.GetItems(spQuery);

                    if (splistItemCollection != null && splistItemCollection.Count > 0)
                        return;

                    SPListItem item = departmentList.Items.Add();
                    item[SPBuiltInFieldId.Title] = siteName;

                    //item.Update();
                    TextInfo UsaTextInfo = new CultureInfo("en-US", false).TextInfo;
                    siteName = UsaTextInfo.ToTitleCase(siteName);

                    

                    string siteUrl = CreateDepartmentSite(web.Url, Constants.Infrastructure.DepartmentSiteTemplateName,
                        siteName.Simplyfied(), siteName, string.Empty, item.ID);

                    item[IOfficeColumnId.DepartmentSiteGroup] = siteName;
                    item[IOfficeColumnId.Department.Link] = siteUrl == null ? string.Empty : siteUrl;
                    item.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_SITE_NAME, siteName.Simplyfied());
                    item.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_ADMIN_GROUP, siteName);
                    item.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_MANAGER_GROUP, siteName);
                    item.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_EMPLOYEE_GROUP, siteName);
                    item.SystemUpdate();
                }
                
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AIA.Intranet.Infrastructure.Data");
            }
        }

        private string CreateDepartmentSite(string url, string tempalteName, string siteName,
            string title, string description, int departmentId)
        {
            string siteUrl = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(url))
                    {
                        site.AllowUnsafeUpdates = true;
                        SPWebTemplateCollection templates = site.GetWebTemplates(1033);
                        var deptsite = templates.Cast<SPWebTemplate>().Where(p => p.Name.Contains(tempalteName)).FirstOrDefault();
                        SPWeb web = site.RootWeb.Webs.Add(siteName, title, description, 1033, deptsite.Name, true, false);
                        siteUrl = web.Url;
                        SetDepartmentSitePermission(web, siteName);
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_DEPARTMENT_ITEM_ID, departmentId.ToString());
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_ADMIN_GROUP, siteName);
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_MANAGER_GROUP, siteName);
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_EMPLOYEE_GROUP, siteName);
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_SITE_NAVIGATION_KEY, siteUrl);
                        web.SetCustomProperty(Constants.DEPARTMENT_CUSTOM_PROPERTIE_SITE_DEPARTMENT_GUID, web.ID.ToString());
                        web.SetCustomProperty(Constants.DEPARTMENT_EMPLOYEE_VIEW, title);
                        web.AllowUnsafeUpdates = true;
                        SPFeature feature = web.Features[new Guid(Constants.DEPARTMENT_INFRASTRUCTURE_FEATURE_ID)];
                        if (feature == null)
                        {
                            web.Features.Add(new Guid(Constants.DEPARTMENT_INFRASTRUCTURE_FEATURE_ID));
                        }
                        web.AllowUnsafeUpdates = false;
                        site.AllowUnsafeUpdates = false;
                    }
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AIA.Intranet.Infrastructure.Data");
            }
            return siteUrl;
        }

        private void SetDepartmentSitePermission(SPWeb web, string managementGroup)
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                var roleManager = web.RoleDefinitions[Constants.APPROVER_PERMISSION_LEVEL];
                //Create group
                web.CreateNewGroup(managementGroup, managementGroup, roleManager);
                web.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AIA.Intranet.Infrastructure.Data");
            }
        }

        /// <summary>
        /// Add user to group
        /// </summary>
        /// <param name="web">web</param>
        /// <param name="groupName">group name to add</param>
        /// <param name="users">login name</param>
        private void AddUserToGroup(SPWeb web, string groupName, List<string> users)
        {
            try
            {
                if (users == null)
                    return;
                SPGroup spGroup = web.Groups[groupName];
                if (spGroup != null)
                {
                    foreach (string user in users)
                    {
                        SPUser spUser = web.EnsureUser(user);
                        if (spUser != null)
                        {
                            spGroup.AddUser(spUser);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "DepartmentEvent");
            }
        }

        private void AddUserToGroup(Guid siteID, Guid departmentSiteID, string groupName, SPUser user)
        {
            if (user == null || string.IsNullOrEmpty(groupName))
                return;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(siteID))
                    {
                        using (SPWeb web = spSite.OpenWeb(departmentSiteID))
                        {
                            SPGroup group = web.SiteGroups[groupName];
                            if (group != null)
                            {
                                group.AddUser(user);
                            }
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AddEmployeeToDepartmentGroup");
            }
        }
        #endregion Functions
    }
}
