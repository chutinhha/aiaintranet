using System;
using System.Linq;
using System.Web;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPSiteExtensions
    {
        public static string GetFeaturePropertyValue(this SPSite site, string featureId, string key)
        {
            var theFeature = site.Features[new Guid(featureId)];
            if (theFeature != null && theFeature.Properties[key] != null)
                return theFeature.Properties[key].Value;
            return string.Empty;
        }

        public static SPPrincipal FindUserOrSiteGroup(this SPSite site, string userOrGroup)
        {
            SPPrincipal myUser = null;

            if (SPUtility.IsLoginValid(site, userOrGroup) || string.Compare(userOrGroup,@"sharepoint\system",true) == 0)
            {
                myUser = site.RootWeb.EnsureUser(userOrGroup);
            }
            else
            {   //might be a group
                foreach (SPGroup g in site.RootWeb.SiteGroups)
                {
                    if (string.Compare(g.Name,userOrGroup,true) == 0)
                        myUser = g;
                }
            }
            return myUser;
        }

        public static SPUser GetUser(this SPSite site, string loginName)
        {
            SPUser myUser = null;

            try
            {
                if (SPUtility.IsLoginValid(site, loginName) || string.Compare(loginName, @"sharepoint\system", true) == 0)
                {
                    myUser = site.RootWeb.EnsureUser(loginName);
                }
            }
            catch { }
            return myUser;
        }

        public static SPUser GetUserByEmail(this SPSite site, string email)
        {
            SPUser myUser = null;
            try
            {
                string loginName = SPUtility.GetLoginNameFromEmail(site, email);
                if (!string.IsNullOrEmpty(loginName))
                {
                    myUser = site.GetUser(loginName);
                }
                else
                {
                    myUser = site.RootWeb.AllUsers.Cast<SPUser>().FirstOrDefault(f => string.Compare(f.Email, email, true) == 0);
                }
            }
            catch { }

            return myUser;
        }

        public static SPList GetListFromURL(this SPSite site, string strURL)
        {
            if (string.IsNullOrEmpty(strURL))
                return null;

            SPSite siteGet = null;
            SPWeb web = null;
            SPList list = null;
            bool disposeSite = false;
            try
            {
                if (Utility.IsAbsoluteUri(strURL))
                    try
                    {
                        siteGet = new SPSite(strURL);
                        web = siteGet.OpenWeb();
                        disposeSite = true;
                    }
                    catch
                    {
                        Utility.LogInfo("Unable to open web from Url : " + strURL + "It isn't SharePoint site or current user don't have permission to open it", "AIA.Intranet");
                    }
                else
                {
                    siteGet = site;
                    web = siteGet.OpenWeb(HttpUtility.UrlDecode(strURL), false);
                }

                try
                {
                    list = web.GetList(strURL);
                }
                catch
                {
                    Utility.LogInfo("Unable to load list from Url : " + strURL, "AIA.Intranet");
                }
            }
            catch
            {
                Utility.LogInfo("Couldn't open " + strURL + " as a SharePoint list", "AIA.Intranet");
            }
            finally
            {
                //if (web != null) web.Dispose();
                //if (disposeSite && siteGet != null) siteGet.Dispose();
            }
            return list;
        }

        public static string CreateSite(this SPSite site, string tempalteName, string siteName, string title, string description)
        {
            string siteDepartmentUrl = string.Empty;
            try
            {
                site.AllowUnsafeUpdates = true;

                SPWebTemplateCollection templates = site.GetWebTemplates(1033);
                var deptsite = templates.Cast<SPWebTemplate>().Where(p => p.Name.Contains(tempalteName)).FirstOrDefault();

                SPWeb web = site.RootWeb.Webs.Add(siteName, title, description, 1033, deptsite.Name, true, false);

                web.Update();
                siteDepartmentUrl = web.Url;

                web.Dispose();
            }
            catch (Exception ex)
            {
                Utility.LogInfo("CreateSite " + ex.ToString(), "AIA.Intranet.Common.Extensions");
            }
            return siteDepartmentUrl;
        }
    }
}
