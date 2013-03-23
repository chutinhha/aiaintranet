using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Services;

namespace AIA.Intranet.Model
{
   public class IOfficeContext
    {
       public static Employee CurentUser
       {
           get
           {
               string loginName = SPContext.Current.Web.CurrentUser.LoginName;
               return EmployeeService.GetEmployeeByUserId(SPContext.Current.Web.CurrentUser.ID);
               
           }
       }

       public static Department Department
       {
           get
           {
               var departmentId = CurentUser.DepartmentId;
               if (departmentId > 0)
               {
                   return DepartmentService.GetDepartment(departmentId);
               }
               return null;
           }
       }

       public static bool IsDepartmentManager
       {
           get
           {
               var currentUser = CurentUser;
               var departmentId = currentUser.DepartmentId;
               var currentUserId = currentUser.ID;
               if (departmentId > 0)
               {
                   Department department = DepartmentService.GetDepartment(departmentId);
                   if (department != null)
                   {
                       if (currentUserId == department.DepartmentHeadId)
                       {
                           return true;
                       }
                       if (department.DeputyDepartmentHeadId.Contains(currentUserId.ToString()))
                       {
                           return true;
                       }
                   }
               }
               return false;
           }
       }

       public static Employee Manager
       {
           get
           {
               var headId = Department.DepartmentHeadId;
               if (headId > 0)
               {
                   return EmployeeService.GetEmployeeByItemId(Department.DepartmentHeadId);
               }
               return null;
           }
       }
    }
}
