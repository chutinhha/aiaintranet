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
using Microsoft.SharePoint.Security;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Controls
{
    public class EmployeeFieldIterator : ListFieldIterator
    {
        protected override bool IsFieldExcluded(SPField field)
        {
            if (SPContext.Current.FormContext.FormMode == SPControlMode.New)
                return base.IsFieldExcluded(field);

            bool isEditItem = base.Item.GetUserEffectivePermissions(SPContext.Current.Web.CurrentUser.LoginName).ToString().Contains(SPBasePermissions.EditListItems.ToString());
            string employeeAccount = SPContext.Current.ListItem[IOfficeColumnId.Employees.Account].ToString();
            string employeeLogin = string.Empty;
            if (!string.IsNullOrEmpty(employeeAccount))
            {
                SPFieldUserValue userValue = new SPFieldUserValue(SPContext.Current.Web, employeeAccount);
                employeeLogin = userValue.User.LoginName;
            }

            List<Guid> incluedViewFields = new List<Guid>()
            {
                SPBuiltInFieldId.Title,
                IOfficeColumnId.Employees.IAvatar,
                IOfficeColumnId.Employees.PhoneNumber,
                IOfficeColumnId.Employees.MobileNumber,
                IOfficeColumnId.Employees.Email,
                IOfficeColumnId.Employees.Nick,
                IOfficeColumnId.Employees.Position,
                IOfficeColumnId.Employees.Gender,
                IOfficeColumnId.Employees.Department
            };

            List<Guid> incluedEditFields = new List<Guid>()
            {
                SPBuiltInFieldId.Attachments,
                IOfficeColumnId.Employees.PhoneNumber,
                IOfficeColumnId.Employees.MobileNumber,
                IOfficeColumnId.Employees.Email,
                IOfficeColumnId.Employees.Nick
            };

            switch (SPContext.Current.FormContext.FormMode)
            {
                case SPControlMode.New:
                    return base.IsFieldExcluded(field);
                case SPControlMode.Edit:
                    return IsExcludedEdit(field, employeeLogin, incluedEditFields);
                case SPControlMode.Display:
                    return IsExcludedDisp(field, isEditItem, incluedViewFields);
                default:
                    break;
            }
            return base.IsFieldExcluded(field);
        }

        private bool IsExcludedEdit(SPField field, string employeeAccount, List<Guid> incluedFields)
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            string currentLogin = currentUser.LoginName;
            //IAvatar & Department Field
            //if (field.Id == IOfficeColumnId.Employees.IAvatar ||
            //    field.Id == IOfficeColumnId.Employees.Department)
            //    return base.IsFieldExcluded(field);

            if (employeeAccount != currentLogin || IsHRUser(currentUser))
                return base.IsFieldExcluded(field);

            if (incluedFields.Contains(field.Id))
                return false;
            return true;
        }

        private bool IsExcludedDisp(SPField field, bool isEditItem, List<Guid> incluedFields)
        {
            string currentLogin = SPContext.Current.Web.CurrentUser.LoginName;

            if (isEditItem || SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                return base.IsFieldExcluded(field);

            if (incluedFields.Contains(field.Id))
                return false;
            return true;
        }

        private bool IsHRUser(SPUser spuser)
        {
            SPGroup group = SPContext.Current.Web.Groups[AIA.Intranet.Resources.CommonResources.HRGroup];
            if (group != null)
            {
                return spuser.InGroup(group);
            }
            return false;
        }
    }
}
