using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Reflection;

namespace AIA.Intranet.Model.Entities
{
    public class Employee : BaseEntity
    {
        public Employee(SPListItem item)
            : base(item)
        {
            try
            {
                Type type = Assembly.Load("AIA.Intranet.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=079ac6f381ab0c9f").GetType("AIA.Intranet.Common.Extensions.SPListItemExtensions");
                MethodInfo method = type.GetMethod("GetCustomProperty");
                TimesheetListGui = method.Invoke(item, new object [] {item, "TIMESHEET_LIST_GUID" }).ToString();

                if (!string.IsNullOrEmpty(Department) && Department.Contains(";#"))
                {
                    var arr = Department.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    DepartmentId = Convert.ToInt32(arr[0]);
                    if (arr.Count() >= 2)
                    {
                        DepartmentName = arr[1];   
                    }
                }

                if (!string.IsNullOrEmpty(Account) && Account.Contains(";#"))
                {
                    var arr = Account.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    UserId = Convert.ToInt32(arr[0]);
                    SPUser spUser = item.ParentList.ParentWeb.AllUsers.GetByID(UserId);
                    LoginName = spUser.LoginName;
                    UserEmail = spUser.Email;
                }

                Fullname = Title;
                string webUrl = item.ParentList.ParentWeb.Url;
                string listId = item.ParentList.ID.ToString();
                ImageUrl = webUrl + "/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item=" + webUrl + "/Lists/Employees/&ID=" + item.ID + "&List=" + listId;

                empoyeeUrl = item.ParentList.ParentWeb.Url + "/Lists/Employees/DispForm.aspx?ID=" + item.ID;

                if (!string.IsNullOrEmpty(Position) && Position.Contains(";#"))
                {
                    var arr = Position.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    PositionId = Convert.ToInt32(arr[0]);
                    if (arr.Count() >= 2)
                    {
                        PositionName = arr[1];
                    }
                }
            }
            catch { }
        }

        public string TimesheetListGui { get; set; }

        public string ImageUrl { get; set; }
        public string empoyeeUrl { get; set; }

        [Field(Ignore=true)]
        public string Fullname { get; set; }
        public string Department { get; set; }
        [Field(Ignore=true)]
        public int DepartmentId { get; set; }
        [Field(Ignore=true)]
        public string DepartmentName { get; set; }

        public string Position { get; set; }
        [Field(Ignore = true)]
        public int PositionId{ get; set; }
        [Field(Ignore = true)]
        public string PositionName { get; set; }

        public string Account { get; set; }
         [Field(Ignore = true)]
        public string LoginName { get; set; }
         [Field(Ignore = true)]
        public int UserId { get; set; }
         [Field(Ignore = true)]
         public string UserEmail { get; set; }

         public int ID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BeginDate { get; set; }
        public string DepartmentSiteGroup { get; set; }
        public string DepartmentAdminGroup { get; set; }
        public string DepartmentManagerGroup { get; set; }
    }
}
