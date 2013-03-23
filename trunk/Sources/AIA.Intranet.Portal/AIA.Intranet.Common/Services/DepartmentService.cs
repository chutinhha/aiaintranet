using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Services
{
    public class DepartmentService
    {
        public static Department GetDepartment(int id)
        {
            var query = new CAMLListQuery<Department>(Constants.DEPARTMENT_LIST_URL);
            //string caml = Camlex.Query()
            //                  .Where(x => x[IOfficeColumnId.Employees.Department] == (DataTypes.LookupId)id.ToString())

            //                  .ToString();

            return query.GetItemById(id);
        }

        public static Department GetDepartment(int id, SPWeb web)
        {
            var list = CCIUtility.GetListFromURL(Constants.DEPARTMENT_LIST_URL, web);
            var query = new CAMLListQuery<Department>(list);
            return query.GetItemById(id);
        }

    }

}
