using System;
using System.Linq;
using AIA.Intranet.Common.Extensions;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Infrastructure.Recievers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class SetAssignementReciever : SPItemEventReceiver
    {
       /// <summary>
       /// An item is being added.
       /// </summary>
       public override void ItemAdding(SPItemEventProperties properties)
       {
           base.ItemAdding(properties);
       }

       /// <summary>
       /// An item is being updated.
       /// </summary>
       public override void ItemUpdating(SPItemEventProperties properties)
       {
           base.ItemUpdating(properties);
       }

       /// <summary>
       /// An item was added.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           var ListItem = properties.ListItem;

           SetPermissionBaseOnAssignmenttField(ListItem);
           base.ItemAdded(properties);
       }

       private void SetPermissionBaseOnAssignmenttField(SPListItem item)
       {
           EventFiringEnabled = false;

           SPField assignemetnField = item.Fields.Cast<SPField>()
                                                 .Where(p => p.Type == SPFieldType.Invalid && p.TypeAsString == "Assignment Field")
                                                 .FirstOrDefault();

           if (assignemetnField != null && item[assignemetnField.Id] != null)
           {
               string assignments = item[assignemetnField.Id].ToString();

               if (assignments.StartsWith("Default")) return;


               if (!item.HasUniqueRoleAssignments)
               {
                   item.BreakRoleInheritance(true);
                   item.SystemUpdate();

                   SPRoleDefinition readerRole = item.Web.RoleDefinitions.GetByType(SPRoleType.Reader);

                   if (assignments.StartsWith("All"))
                   {

                       item.RemoveReadPermissions();
                       item.SetPermissions(readerRole.Name, "NT AUTHORITY\\Authenticated users");
                       item.SystemUpdate();
                   }
                   else
                   {
                       var names = assignments.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                       List<string> loginAccounts = new List<string>();
                       SPList list = item.ParentList;

                       if (names.Length > 1)
                       {
                           for (int i = 0; i < names.Length; i = i + 2)
                           {
                               SPListItemCollection items = list.FindItems("ID", names[i]);
                               //loginAccounts.Add(items[0][EmployeeResources.InternalAccount].ToString().Trim());
                           }
                       }

                       item.RemoveReadPermissions();
                       item.SetPermissions(readerRole.Name, loginAccounts);
                   }
               }
           }

           EventFiringEnabled = true;
       }

       /// <summary>
       /// An item was updated.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
           base.ItemUpdated(properties);
       }


    }
}
