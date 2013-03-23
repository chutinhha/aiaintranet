using System;
using System.Linq;
using AIA.Intranet.Infrastructure.Models;
using Microsoft.SharePoint;
using System.Web;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Infrastructure.Utilities
{
    public class SharepointHelper
    {
        public static ADSetting GetADSetting()
        {
            ADSetting settings = SPContext.Current.Site.RootWeb.GetCustomSettings<ADSetting>(IOfficeFeatures.Infrastructure, false);
            if (settings != null)
            {
                if (settings.Password != null && !string.IsNullOrEmpty(settings.Password))
                {
                    try
                    {
                        settings.Password = CryptoHelper.Decrypt(settings.Password);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex.Message);
                    }
                    
                }
            }
            
            return settings;
        }

        public static ADSetting GetADSettingWithOU()
        {
            ADSetting settings = SharepointHelper.GetADSetting();
            if (settings != null)
            {

                settings.OUValue = SharepointHelper.GetCurrentOU();
            }

            return settings;
        }

        public static string GetCurrentOU()
        {
            //SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            string result = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb rootWeb = site.OpenWeb())
                    {
                        try
                        {
                            SPList aDManagerList = rootWeb.GetList(rootWeb.Url.TrimEnd('/') + "/" + Constants.ADManagerListUrl);
                            string currentWebUrl = SPContext.Current.Web.Url.TrimEnd('/');
                            SPListItemCollection items = aDManagerList.Items;
                            string configUrl = string.Empty;
                            foreach (SPListItem item in items)
                            {
                                SPFieldUrlValue value = new SPFieldUrlValue(item[Constants.UrlField].ToString());
                                configUrl = value.Url;
                                //if (configUrl.Contains(".aspx"))
                                //{
                                //    configUrl = configUrl.Substring(0, configUrl.LastIndexOf("/"));

                                //}
                                configUrl = configUrl.TrimEnd('/');
                                configUrl = HttpUtility.UrlDecode(configUrl);
                                if (string.Equals(currentWebUrl, configUrl, StringComparison.OrdinalIgnoreCase))
                                {
                                    result = item[Constants.OUField].ToString();
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Write(ex);
                        }
                    }
                }

            });


            return result;
        }

        public static bool HasPermissions(SPWeb web, string[] permissions)
        {
            // Get a reference to the roles that 
            // are bound to the user and the role 
            // definition against which we need to 
            // verify the user. 
            SPRoleDefinitionBindingCollection usersRoles = web.AllRolesForCurrentUser;
            
            //SPRoleDefinitionCollection siteRoleCollection = web.RoleDefinitions;



            var existingPermission = usersRoles.Cast<SPRoleDefinition>().Where(r => permissions.Contains(r.Name));
            System.Diagnostics.Debug.WriteLine("---------------" + existingPermission.Count());
            return (existingPermission.Count() > 0);

            //SPRoleDefinition fullRoleDefinition = siteRoleCollection["Full Control"];
            //SPRoleDefinition designRoleDefinition = siteRoleCollection["Design"];
            
            // Determine whether the user is in the role. If 
            // not, redirect the user to the access-denied page 
            //if (!(usersRoles.Contains(fullRoleDefinition) || usersRoles.Contains(designRoleDefinition)))
            //{
            //    result = false;
            //}
        }

    }
}
