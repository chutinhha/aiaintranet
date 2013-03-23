using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Model;
using System.Collections.Generic;
using AIA.Intranet.Common.Utilities;
using System.Linq;


namespace AIA.Intranet.Infrastructure.Recievers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class UnreadContentReciever : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;

            if (item.ModerationInformation == null || item.ModerationInformation.Status == SPModerationStatusType.Approved)
            {
                UnreadContentNotificationSetting setting = properties.List.GetCustomSettings<UnreadContentNotificationSetting>(IOfficeFeatures.Infrastructure);
                if (setting == null || setting.Enable == false) return;

                if (setting.EnableEmail)
                {
                    SendNotificationEmail(setting.Template, item);
                }

                if (setting.EnableCreateUnreadTask)
                {
                    CreateNotificationTask(setting.TitleFormula, item);
                }
            }

            base.ItemAdded(properties);
        }

        public static void CreateNotificationTask(string formula, SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                var title = item.GetFormulaValue(formula);
                var principals = item.GetEffectivePrincipals(SPBasePermissions.ViewListItems);
                List<SPUser> users = new List<SPUser>();

                foreach (var principal in principals)
                {
                    SPUser user = null;
                    if (principal.LoginName == "NT AUTHORITY\\authenticated users")
                    {
                        users = item.Web.AllUsers.Cast<SPUser>().Where(p => p.Name != "Authenticated Users").ToList();
                        break;
                    }
                    try
                    {
                        user = item.Web.EnsureUser(principal.LoginName);
                    }

                    catch { }
                    if (user != null) { users.Add(user); }


                    else
                    {
                        try
                        {
                            SPGroup group = item.Web.Groups[principal.Name];
                            if (group != null)
                            {
                                var listUsers = group.Users.Cast<SPUser>().ToList();

                                //test
                                var authenticatedUsersObj = listUsers.Where(s => s.LoginName == "NT AUTHORITY\\authenticated users").ToList();
                                if (authenticatedUsersObj.Count > 0)
                                {
                                    users = item.Web.AllUsers.Cast<SPUser>().Where(p => p.Name != "Authenticated Users").ToList();
                                    break;
                                }
                                else
                                {
                                    users.AddRange(listUsers);
                                }
                            }

                        }
                        catch { }
                    }

                }
                var grouped = users.GroupBy(p => p.LoginName);


                using (var site = new SPSite(item.Web.Site.ID))
                using (var web = site.OpenWeb())
                {
                    SPList unreadTaskList = CCIUtility.GetListFromURL(Constants.UNREAD_CONTENT_LIST_URL, web);
                    foreach (var group in grouped)
                    {
                        var user = group.First();

                        if (user.LoginName == "SHAREPOINT\\system") continue;

                        var taskItem = unreadTaskList.AddItem();
                        taskItem[SPBuiltInFieldId.Title] = title;
                        taskItem[IOfficeColumnId.RelatedContent] = new SPFieldUrlValue(item.DisplayFormUrl());
                        taskItem[SPBuiltInFieldId.AssignedTo] = user;
                        taskItem[SPBuiltInFieldId.TaskDueDate] = DateTime.MaxValue;
                        taskItem[SPBuiltInFieldId.StartDate] = DateTime.Now;
                        taskItem.SystemUpdate();

                        taskItem.BreakRoleInheritance(false);
                        taskItem.SetPermissions(user, SPRoleType.Reader);
                    }
                };

            });
        }

        public static void SendNotificationEmail(EmailTemplateSettings emailTemplateSettings, SPListItem sPListItem)
        {

        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }


    }
}
