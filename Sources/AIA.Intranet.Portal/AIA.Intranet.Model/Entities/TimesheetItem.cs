using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class TimesheetItem : BaseEntity
    {
        public TimesheetItem(SPListItem item)
            : base(item)
        {
            try
            {
                SPWeb web = SPContext.Current.Web;

                if (!string.IsNullOrEmpty(Employee))
                {
                    SPFieldLookupValue lookupValue = new SPFieldUserValue(web, Employee);
                    EmployeeId = lookupValue.LookupId;
                }

                if (!string.IsNullOrEmpty(DepartmentSiteColumn))
                {
                    SPFieldLookupValue lookupValue = new SPFieldUserValue(web, DepartmentSiteColumn);
                    DepartmentId = lookupValue.LookupId;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string TimesheetTask { get; set; }
        public string TimesheetProject { get; set; }
        public string TypeOfWork { get; set; }
        public string WorkTime { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Week { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }

        public string Employee { get; set; }
        [Field(Ignore = true)]
        public int EmployeeId { get; set; }

        public string DepartmentSiteColumn { get; set; }
        [Field(Ignore = true)]
        public int DepartmentId { get; set; }

        public string IsSync { get; set; }

        public string Comments { get; set; }
    }
}
