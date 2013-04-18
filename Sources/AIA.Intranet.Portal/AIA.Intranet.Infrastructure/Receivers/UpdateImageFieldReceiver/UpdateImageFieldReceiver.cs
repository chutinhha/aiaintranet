using System;
using System.Linq;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using AIA.Intranet.Infrastructure.CustomFields;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.Receivers
{
    /// <summary>
    /// List Events
    /// </summary>
    public class UpdateImageFieldReceiver : SPListEventReceiver
    {
       /// <summary>
       /// A list was added.
       /// </summary>
       public override void ListAdded(SPListEventProperties properties)
       {
           base.ListAdded(properties);
           this.EventFiringEnabled = false;
           try
           {
               var fields = properties.List.Fields.Cast<SPField>().ToList();

               var imageFields = fields.Where(p => p.TypeAsString == Constants.IMAGE_FIELD_TYPE_NAME).ToList();
               //var lkwithPickerFields = fields.Where(p => p.TypeAsString == Constants.LOOKUP_WITH_PICKER_TYPE_NAME).ToList();

               foreach (var item in imageFields)
               {
                   var currentWeb = item.ParentList.ParentWeb;
                   var list = currentWeb.Lists[currentWeb.Folders["NewsImages"].ParentListId];

                   item.UpdateImageField(item.ParentList.ParentWeb, list);
               }

               /*
               foreach (LookupFieldWithPicker item in lkwithPickerFields)
               {
                   if (!item.LookupList.IsGuid())
                   {
                       var root = properties.List.ParentWeb.Site.RootWeb;
                       var list = root.GetList(item.LookupList);
                       System.Collections.Generic.Dictionary<string, object> props = new System.Collections.Generic.Dictionary<string, object>();
                       props.Add("WebId", root.ID);
                       props.Add("ListId", list.ID);
                       item.UpdateProperties(props);
                   }
               }
               */
           }
           catch (Exception ex)
           {
           }
           finally
           {
               this.EventFiringEnabled = false;
           }
       }
    }
}
