using System.Linq;
using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Utilities;
using System.Collections.Generic;
using System.Collections;
using Microsoft.SharePoint.WebControls;
using System.Collections.Specialized;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Infrastructure.CustomFields;
using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Infrastructure.Recievers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class NotificationReciever : SPItemEventReceiver
    {
       /// <summary>
       /// An item is being added.
       /// </summary>
      
       public override void ItemAdded(SPItemEventProperties properties)
       {
           //var parentList = properties.ListItem.ParentList;
           var settings = properties.ListItem.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);

           if (settings == null || settings.Settings.Count ==0)
           {
               return;
           }

           foreach (var item in settings.Settings)
           {
               if (item.RunOnCreated && item.Enable)
               {
                   HandleNotification(item, properties.ListItem);
               }
           }
           
           base.ItemAdded(properties);
       }
       public override void ItemUpdated(SPItemEventProperties properties)
       {
           //var parentList = properties.ListItem.ParentList;
           var settings = properties.ListItem.GetCustomSettings<NotificationSettingsCollection>(IOfficeFeatures.Infrastructure);

           if (settings == null || settings.Settings.Count == 0)
           {
               return;
           }

           foreach (var item in settings.Settings)
           {
               if (item.RunOnChanged && item.Enable)
               {
                   HandleNotification(item, properties.ListItem);
               }
           }
          
           base.ItemUpdated(properties);
       }
       private bool ValidateItem(SPListItem item, List<Criteria> conditions)
       {
           foreach (Criteria c in conditions)
           {
               Guid fieldId = new Guid(c.FieldId);
               if (item[fieldId] == null) return false;

               if (FieldValueHelper.ValidateItemProperty(item, fieldId, c))
                   continue;

               return false;
           }
           return true;
       }
       private void HandleNotification(NotificationSettings setting, SPListItem currentItem)
       {
           if(!setting.Enable || (setting.Enable && setting.EnableCondition && !ValidateItem(currentItem, setting.Conditions)) ) return;

           var emailTemplate = TemplateHelper.PopulateTemplate(setting.Template.Url, currentItem.ParentList.ParentWeb.Site.RootWeb, setting.Template.Name, currentItem);
           if (emailTemplate == null) return;
           var addresses = GetEmailAddressFromSetting(setting, currentItem);
           var cc = GetCCEmailAddressFromSetting(setting, currentItem);
           try
           {
               string to = string.Empty;
               foreach (var item in addresses)
               {
                   to += item + ",";
               }

               string ccaddress = string.Empty;
               foreach (var item in cc)
               {
                   ccaddress += item + ",";
               }

               StringDictionary headers = new StringDictionary();
               headers.Add("to", to);
               headers.Add("cc", ccaddress);
               headers.Add("bcc", "");
               //headers.Add("from", "email@add.com");
               headers.Add("subject", emailTemplate.Subject);
               headers.Add("content-type", "text/html");
               SPUtility.SendEmail(currentItem.Web, headers, emailTemplate.Body);

               //foreach (var item in addresses)
               //{


               //    SPUtility.SendEmail(currentItem.Web, headers, emailTemplate.Body);

               //    SPUtility.SendEmail(currentItem.Web, true, emailTemplate.SendAsPlainText, item, emailTemplate.Subject, emailTemplate.Body);

               //}
           }
           catch (Exception ex)
           {

               CCIUtility.LogError(ex.Message, IOfficeFeatures.Infrastructure);
           }

       }

       private List<string> GetEmailAddressFromSetting(NotificationSettings setting, SPListItem item)
       {
           List<string> results =  new List<string>();
           if (setting.SendToAll)
           {
               foreach (SPUser user in item.Web.SiteUsers)
               {
                   if (!string.IsNullOrEmpty(user.Email) &&
                       user.Email.IsValidEmailAddress() &&
                       !results.Contains(user.Email))
                       results.Add(user.Email);
                   
               }
           }

           if (setting.SendToSpecificUsers)
           {
               foreach (var loginanme in setting.SelectedUserOrGroups)
               {
                   var siteUser = item.Web.SiteUsers[loginanme];

                    if (siteUser!= null && 
                        !string.IsNullOrEmpty(siteUser.Email) &&
                       siteUser.Email.IsValidEmailAddress() &&
                       !results.Contains(siteUser.Email))

                       results.Add(siteUser.Email);
               }
           }

           if (setting.SendToSelectedColumns)
           {
               var emails = GetAddressFromMetadata(setting.SelectedColumns, item);
               results.AddRange(emails);
           }
           if (setting.SendToMaillist && setting.Maillists!= null)
           {
               var items = EmailListService.GetAllEmailListItems(item.ParentList.ParentWeb.Site.RootWeb);
               foreach (var mlItem in items)
               {
                   if(setting.Maillists.Contains(mlItem.Title)){
                       results.Add(mlItem.AllEmails);
                   }
               }
           }

           return results;
       }
       private List<string> GetCCEmailAddressFromSetting(NotificationSettings setting, SPListItem item)
       {
           List<string> results = new List<string>();
           
               foreach (var loginanme in setting.CCUserOrGroups)
               {
                   
                   var siteUser = item.Web.EnsureUser(loginanme);

                   if (siteUser != null &&
                       !string.IsNullOrEmpty(siteUser.Email) &&
                      siteUser.Email.IsValidEmailAddress() &&
                      !results.Contains(siteUser.Email))

                       results.Add(siteUser.Email);
               }
               var emails = GetAddressFromMetadata(setting.CCColumns, item);
           

            results.AddRange(emails);
            return results;
       }

       private List<string>  GetAddressFromMetadata(List<string> columns, SPListItem item)
       {
           List<string> results = new List<string>();
           foreach (var column in columns)
           {
               Guid columnId = new Guid(column);
               if (item.Fields.ContainFieldId(columnId))
               {
                   switch (item.Fields[columnId].TypeAsString)
                   {
                       case "LookupFieldWithPicker": //Use for LookupFieldWithPicker using EmployeeList only
                           {
                               SPFieldLookupValue user = (SPFieldLookupValue)item[columnId];
                               if (user != null)
                               {
                                   Employee employee = EmployeeService.GetEmployeeByItemId(user.LookupId, item.ParentList.ParentWeb);
                                   results.Add(employee.UserEmail);
                               }
                           }
                           break;
                       case "LookupFieldWithPickerMulti": //Use for LookupFieldWithPicker using EmployeeList only
                           {
                               SPFieldLookupValueCollection users = (SPFieldLookupValueCollection)item[columnId];
                               if (users != null && users.Count > 0)
                               {
                                   foreach (var user in users)
                                   {
                                       Employee employee = EmployeeService.GetEmployeeByItemId(user.LookupId, item.ParentList.ParentWeb);
                                       results.Add(employee.UserEmail);
                                   }
                               }
                           }
                           break;
                       case "AssignmentField":
                           {
                               string approver = item[columnId] as string;
                               var names = approver.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);

                               if (names.Length > 1)
                               {
                                   for (int i = 0; i < names.Length; i = i + 2)
                                   {
                                       SPUser user = item.Web.AllUsers.GetByID(int.Parse(names[i]));
                                       results.Add(user.LoginName);
                                   }
                               }
                           }
                           break;
                       default:
                           {
                               string stringValue = item[columnId] as string;
                               var count = stringValue.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                               if (count.Length > 2)
                               {
                                   var members = new SPFieldUserValueCollection(item.ParentList.ParentWeb, stringValue);
                                   if (members != null)
                                   {
                                       for (int i = 0; i < members.Count; i++)
                                       {
                                           results.Add(members[i].User.Email);
                                       }
                                   }
                               }
                               else
                               {
                                   var member = new SPFieldUserValue(item.ParentList.ParentWeb, stringValue);
                                   if (member != null)
                                   {
                                        results.Add(member.User.Email);
                                   }
                               }
                           }
                           break;
                   }
               }
           }
           return results;
       }
    }
}
