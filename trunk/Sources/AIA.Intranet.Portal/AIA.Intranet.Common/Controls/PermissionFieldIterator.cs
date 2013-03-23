using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Infrastructure;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AIA.Intranet.Common.Controls
{
    public class PermissionFieldIterator : ListFieldIterator
    {
        
        protected override bool IsFieldExcluded(SPField field)
        {

            //SetFieldInputReadonly(field);
            //field.FieldRenderingControl.DisableInputFieldLabel = true;
            //field.FieldRenderingControl.RenderContext.FormContext.FormMode = SPControlMode.Display;

            var setting = SPContext.Current.List.GetCustomSettings<FieldPermissionSettings>(Model.IOfficeFeatures.IOfficeApp);

            if (setting == null && setting[field.Id] == null)
            {
                return base.IsFieldExcluded(field);
            }
            switch (SPContext.Current.FormContext.FormMode)
            {
                case SPControlMode.Display:
                    return IsExcludedBySetting(field, setting[field.Id].ViewGroupPermission, setting[field.Id].ViewUserPermissions);
                    break;
                case SPControlMode.Edit:
                    return IsExcludedBySetting(field, setting[field.Id].EditGroupPermissions, setting[field.Id].EditUserPermissions);
                    break;
                case SPControlMode.Invalid:
                    break;
                case SPControlMode.New:
                    return IsExcludedBySetting(field, setting[field.Id].NewGroupPermissions, setting[field.Id].NewUserPermissions);
                    break;
                default:
                    break;
            }

            return base.IsFieldExcluded(field);

        }

        private void SetFieldInputReadonly(SPField field)
        {
            foreach (Control item in field.FieldRenderingControl.Controls)
            {
                SetReadOnly(item);
            }

        }

        private void SetReadOnly(Control control)
        {
            try
            {
                WebControl wc = control as WebControl;
                wc.Enabled = false;
                wc.Attributes.Add("readonly", "readonly");
                wc.Attributes.Add("disabled", "true");
            }
            catch (Exception)
            {
                
                
            }
            foreach (Control item in control.Controls)
            {
                SetReadOnly(item);
            }
        }
        
        private bool IsExcludedBySetting(SPField field, List<string> groups, List<string> usersandGroups)
        {
            var currentUser = SPContext.Current.Web.CurrentUser;
            var spGroups = currentUser.Groups.Cast<SPGroup>().ToList();

            if (spGroups.Exists(p => groups.Any(p1 => p1 == p.Name))) return true;

            foreach (var item in usersandGroups)
            {
                SPUser user = SPContext.Current.Web.EnsureUser(item);
                
                if (user != null && user.ID == SPContext.Current.Web.CurrentUser.ID) return true;

                //Add additional code to handle Group
            }

            //SetFieldInputReadonly(field);

            return base.IsFieldExcluded(field);
        }

    }
}
