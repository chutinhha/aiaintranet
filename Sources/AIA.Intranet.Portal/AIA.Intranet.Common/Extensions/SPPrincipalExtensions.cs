using System;
//using AIA.Intranet.Common.Utilities;
//using AIA.Intranet.Model.Infrastructure;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPPrincipalExtensions
    {
        public static CCIProxy GetProxyUser(this SPPrincipal principal, SPSite site)
        {
            CCIProxy proxy = new CCIProxy();
            SPList proxyList = CCIUtility.GetListFromURL(site.RootWeb.Url + "/Lists/CCIappProxySettings");
            if (proxyList == null)
                return null;

            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name=\"Owner\"  LookupId=\"TRUE\"/><Value Type=\"Integer\" >{0}</Value></Eq></Where>", principal.ID);
            SPListItemCollection items = proxyList.GetItems(query);

            if (items == null || items.Count == 0)
                return null;

            SPListItem item = items[0];
            if (item["ProxyStatus"] == null)
                return null;

            string proxyStatus = item["ProxyStatus"].ToString();
            if (string.Compare(proxyStatus, "Inactive", true) == 0)
                return null;

            if (item["ProxyStartDate"] == null || item["ProxyEndDate"] == null)
                return null;

            DateTime now = DateTime.Now;
            DateTime startDate = Convert.ToDateTime(item["ProxyStartDate"]);
            DateTime endDate = Convert.ToDateTime(item["ProxyEndDate"]);

            if (startDate <= now && now <= endDate)
            {
                if (item["DelegateUser"] == null)
                    return null;

                SPFieldUserValue delegateUser = new SPFieldUserValue(site.RootWeb, (string)item["DelegateUser"]);
                if (delegateUser.User != null)
                {
                    proxy.DelegateUser.LoginName = delegateUser.User.LoginName;
                    proxy.DelegateUser.Email = delegateUser.User.Email;
                }
                else
                    proxy.DelegateUser.LoginName = delegateUser.LookupValue;

                if (item["CCUsers"] != null)
                {
                    SPFieldUserValueCollection ccUsers = (SPFieldUserValueCollection)item["CCUsers"];
                    foreach (SPFieldUserValue ccUser in ccUsers)
                    {
                        if (ccUser.User == null)
                            continue;
                        CCIUser user = new CCIUser();
                        user.LoginName = ccUser.User.LoginName;
                        user.Email = ccUser.User.Email;
                        proxy.CCUser.Add(user);
                    }
                }

                if (item["CCSubjectEmail"] != null)
                    proxy.CCSubjectEmail = item["CCSubjectEmail"].ToString();

                if (item["CCBodyEmail"] != null)
                    proxy.CCBodyEmail = item["CCBodyEmail"].ToString();

            }
            return proxy;
        }
    }
}
