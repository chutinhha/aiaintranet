using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPUserExtensions
    {
        public static bool InGroup(this SPUser spUser, SPGroup spGroup)
        {
            return spUser.Groups.Cast<SPGroup>()
              .Any(g => g.ID == spGroup.ID);
        }


        public static void AddUserToGroup(this SPUser spUser, string groupName, SPWeb spWeb)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(spWeb.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(spWeb.ID))
                    {
                        SPGroup spGroup = web.SiteGroups[groupName];
                        if (!spUser.InGroup(spGroup))
                        {
                            web.AllowUnsafeUpdates = true;
                            spGroup.AddUser(spUser);
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }

        public static void LeaveGroup(this SPUser spUser, string groupName, SPWeb spWeb)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(spWeb.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(spWeb.ID))
                    {
                        SPGroup spGroup = web.SiteGroups[groupName];
                        if (spUser.InGroup(spGroup))
                        {
                            web.AllowUnsafeUpdates = true;
                            spGroup.RemoveUser(spUser);
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }
    }
}
