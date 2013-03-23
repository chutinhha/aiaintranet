using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class EmailListItem : BaseEntity
    {
        #region Properties
        public string Emails { get; set; }
        public string UserEmails { get; set; }
        public string AllEmails { get; set; }

        #endregion


        public EmailListItem(DataRow item)
            : base(item)
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
        }

        public EmailListItem(SPListItem item)
            : base(item)
        {
            try
            {
                string userEmails = string.Empty;
                bool breakLoop = false;

                if (item["Users"] != null && !string.IsNullOrEmpty(item["Users"].ToString()))
                {
                    SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(item.Web.Site.RootWeb, item["Users"].ToString());
                    foreach (SPFieldUserValue userValue in userValues)
                    {
                        if (userValue.User == null)
                        {
                            SPGroup group = item.Web.Site.RootWeb.SiteGroups.GetByID(userValue.LookupId);
                            foreach (SPUser user in group.Users)
                            {
                                if (user.LoginName == "NT AUTHORITY\\authenticated users")
                                {
                                    userEmails = string.Empty;
                                    foreach (SPUser siteUser in item.Web.Site.RootWeb.SiteUsers)
                                    {
                                        if (!string.IsNullOrEmpty(siteUser.Email))
                                            userEmails += siteUser.Email + ",";
                                    }

                                    breakLoop = true;
                                    break;
                                }

                                if (!string.IsNullOrEmpty(user.Email))
                                {
                                    userEmails += user.Email + ",";
                                }
                                   
                            }

                            if (breakLoop) break;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(userValue.User.Email))
                                userEmails += userValue.User.Email + ",";
                        }
                    }

                    UserEmails = userEmails.Trim(','); 
                }

                if (Emails == null) Emails = string.Empty;

                List<string> listEmails = Emails.Split(new string[] { ", "}, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> listUserEmails = userEmails.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> listAllEmails = listEmails.Union(listUserEmails).ToList();

                AllEmails = string.Join(",", listAllEmails.ToArray());
            }
            catch (Exception ex)
            {
            }
        }

    }
}
