using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using System.IO;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Security;
using AIA.Intranet.Model.Search;
using AIA.Intranet.Model.Security;

namespace AIA.Intranet.Common
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class SecurityEventHandler : SPItemEventReceiver
    {
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            SPListItem listItem = properties.ListItem;

            HandleSecurityRules(listItem, properties.EventType);
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            SPListItem listItem = properties.ListItem;

            HandleSecurityRules(listItem, properties.EventType);
        }

        public void HandleSecurityRules(SPListItem listItem, SPEventReceiverType eventType)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SecuritySettings settings = listItem.ParentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);

                if (settings == null) return;
                settings.Rules.Sort(delegate(Rule rule1, Rule rule2)
                {
                    return rule1.Order.CompareTo(rule2.Order);
                });

                foreach (Rule r in settings.Rules)
                {
                    bool ruleQualified = ValidateRule(r, listItem, eventType);
                    if (ruleQualified)
                    {
                        bool itemQuailfied = ValidateItem(listItem, r);
                        if (itemQuailfied)
                        {
                            ApplySecuritySettings(listItem, r);
                            break;
                        }
                    }
                }
            });
        }

        private bool ValidateRule(Rule r, SPListItem item, SPEventReceiverType eventType)
        {
            if (r.RunOnAdded && eventType == SPEventReceiverType.ItemAdded)
                return true;

            if (r.RunOnAnyUpdate == true &&
                eventType == SPEventReceiverType.ItemUpdated &&
                item.Level != SPFileLevel.Checkout)
                return true;

            if (r.RunOnFirstUpdate == true &&
                eventType == SPEventReceiverType.ItemUpdated &&
                item.Level != SPFileLevel.Checkout)
            {
                if (item[SPBuiltInFieldId.owshiddenversion].ToString() == "2")
                {
                    return true;
                }
            }
            return false;
        }

        private void ApplySecuritySettings(SPListItem item, Rule r)
        {
            List<PermissionAssigment> permissionAssignments = r.PermissionAssignments;
            using (SPSite site = new SPSite(item.Web.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(item.Web.ID))
                {
                    SPListItem listItem = web.GetListItem(web.Url + "/" + item.Url);
                    SPUser owner = listItem.GetOwner();
                    if (!r.PreserveExistingSecurity)
                    {
                        listItem.RemoveAllPermissions();
                    }
                    if (!string.IsNullOrEmpty(r.OwnerPermission))
                        listItem.SetPermissions(r.OwnerPermission, owner.LoginName);

                    foreach (PermissionAssigment pa in permissionAssignments)
                    {
                        setPerrmissionToListItem(listItem, pa);
                    }

                    //remove system account
                    if (listItem.HasUniqueRoleAssignments)
                    {
                        //RunWithElevatedPrivileges => CurrentUser is System Account
                        SPPrincipal sysAccount = web.CurrentUser;
                        listItem.RoleAssignments.Remove(sysAccount);
                    }
                }
            }
        }

        private static void setPerrmissionToListItem(SPListItem listItem, PermissionAssigment pa)
        {
            List<string> members = new List<string>();
            if (pa.Members != null && pa.Members.Count > 0)
            {
                members.AddRange(pa.Members);
            }

            if (pa.FieldIds != null && pa.FieldIds.Count > 0)
            {
                foreach (string fieldId in pa.FieldIds)
                {
                    if (listItem.Fields.ContainFieldId(new Guid(fieldId)) && listItem[new Guid(fieldId)] != null)
                    {
                        SPFieldUserValueCollection users = new SPFieldUserValueCollection(listItem.Web, listItem[new Guid(fieldId)].ToString());
                        foreach (SPFieldUserValue user in users)
                        {
                            if (user.User != null)
                                members.Add(user.LookupId + ";#" + user.User.LoginName);
                            else
                                members.Add(user.ToString());
                        }
                    }
                }
            }

            listItem.SetPermissions(pa.PermissionLevel, members);
        }


        private bool ValidateItem(SPListItem item, Rule rule)
        {
            if (!string.IsNullOrEmpty(rule.ContentTypeId) &&
                item[SPBuiltInFieldId.ContentTypeId].ToString() != rule.ContentTypeId)
                return false;

            if (!validateDocumentTypes(item, rule))
            {
                return false;
            }

            foreach (Criteria c in rule.CriteriaList)
            {
                Guid fieldId = new Guid(c.FieldId);
                if (item[fieldId] == null) return false;

                if (FieldValueHelper.ValidateItemProperty(item, fieldId, c))
                    continue;

                return false;
            }
            return true;
        }

        private bool validateDocumentTypes(SPListItem item, Rule rule)
        {
            if (!string.IsNullOrEmpty(rule.DocumentTypes) && item.File != null)
            {
                string ext = item.File.Name.Substring(item.File.Name.LastIndexOf('.') + 1).ToLower();

                foreach (string srtExt in rule.DocumentTypes.Split(new char[] { ';' }))
                {
                    if (string.Compare(ext, srtExt.Trim(), true) == 0)
                    {
                        return true;
                    }
                }
                // If don't match any document type 
                return false;

            }
            return true;
        }
    }
}
