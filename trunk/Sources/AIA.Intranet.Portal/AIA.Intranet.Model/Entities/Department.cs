using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class Department: BaseEntity
    {
        public Department(SPListItem item)
            : base(item)
        {
            try
            {
                SPFieldLookupValue valueDeptHead = item[IOfficeColumnId.Department.DepartmentHead] as SPFieldLookupValue;
                if (valueDeptHead != null)
                {
                    DepartmentHeadId = valueDeptHead.LookupId;
                }
                
                SPFieldLookupValueCollection valueDeputyDeptHead = item[IOfficeColumnId.Department.DeputyDepartmentHead] as SPFieldLookupValueCollection;
                DeputyDepartmentHeadId = string.Empty;
                if (valueDeputyDeptHead != null)
                {
                    for (int i = 0; i < valueDeputyDeptHead.Count; i++)
                    {
                        DeputyDepartmentHeadId += valueDeputyDeptHead[i].ToString().Split(';')[0] + "|";
                    }
                }

                IsDepartmentHead = false;
                IsDeputyDepartmentHead = false;
                if (valueDeptHead != null)
                {
                    IsDepartmentHead = true;
                }
                if (valueDeputyDeptHead != null && valueDeputyDeptHead.Count > 0)
                {
                    IsDeputyDepartmentHead = true;
                }                
            }
            catch (Exception)
            {

            }
        }

        public string DepartmentHead { get; set; }
        [Field(Ignore = true)]
        public bool IsDepartmentHead { get; set; }
        [Field(Ignore = true)]
        public int DepartmentHeadId { get; set; }

        public string DeputyDepartmentHead { get; set; }
        [Field(Ignore = true)]
        public bool IsDeputyDepartmentHead { get; set; }
        [Field(Ignore = true)]
        public string DeputyDepartmentHeadId { get; set; }

        public string DepartmentSiteGroup { get; set; }
        public string Link { get; set; }

        //public int DeptHeadId { get; set; }
        //public string DeputyDeptHeadId { get; set; }
        //public bool isDeptHead { get; set; }
        //public bool isDeputyDeptHead { get; set; }
        //public string DeptSiteGroup { get; set; }
    }
}
