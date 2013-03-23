using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Services
{
    public class EmployeeService
    {

        public static Employee GetEmployeeByLoginName(string loginName)
        {
            
            var query = new CAMLListQuery<Employee>(Constants.EMPLOYEE_LIST_URL);
            string caml = Camlex.Query()
                              .Where(x => x[IOfficeColumnId.Employees.Account] == (DataTypes.User)loginName)

                              .ToString();

            return query.ExecuteSingleQuery(caml);
        }

        public static Employee GetEmployeeByUserId(int userId)
        {
            var query = new CAMLListQuery<Employee>(Constants.EMPLOYEE_LIST_URL);
            string caml = Camlex.Query()
                              .Where(x => x[IOfficeColumnId.Employees.Account] == (DataTypes.LookupId)userId.ToString())

                              .ToString();

            return query.ExecuteSingleQuery(caml);
        }

        public static Employee GetEmployeeByUserId(int userId, SPWeb web)
        {
            SPList empList = web.GetList(Constants.EMPLOYEE_LIST_URL);

            var query = new CAMLListQuery<Employee>(empList);
            string caml = Camlex.Query()
                              .Where(x => x[IOfficeColumnId.Employees.Account] == (DataTypes.LookupId)userId.ToString())

                              .ToString();

            return query.ExecuteSingleQuery(caml);
        }

        public static Employee GetEmployeeByItemId(int id)
        {
            var query = new CAMLListQuery<Employee>(Constants.EMPLOYEE_LIST_URL);
            return query.GetItemById(id);
        }

        public static Employee GetEmployeeByItemId(int id, SPWeb web)
        {
            var list = CCIUtility.GetListFromURL(Constants.EMPLOYEE_LIST_URL, web);
            var query = new CAMLListQuery<Employee>(list);
            return query.GetItemById(id);
        }

        public static void SetEmployeeDepartment(int itemId, int departmentId, string departmentSiteGroup, SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(web.Site.ID))
                {
                    using (SPWeb spWeb = site.OpenWeb(web.ID))
                    {
                        var list = CCIUtility.GetListFromURL(Constants.EMPLOYEE_LIST_URL, spWeb);
                        using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                        {
                            var item = list.GetItemById(itemId);
                            item[IOfficeColumnId.Employees.Department] = departmentId;
                            item[IOfficeColumnId.DepartmentSiteGroup] = departmentSiteGroup;
                            item.SystemUpdate();
                        }
                    }
                }
            });

            
        }
        
    }
}
