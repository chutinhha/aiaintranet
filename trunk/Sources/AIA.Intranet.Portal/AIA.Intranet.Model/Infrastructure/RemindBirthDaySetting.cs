using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class RemindBirthDaySetting
    {
        public bool IsEnableRemindBirthDay { get; set; }
        public bool IsSendByDay { get; set; }
        public bool IsSendByWeek { get; set; }
        public bool IsSendByMonth { get; set; }
        public bool IsSendMail { get; set; }
        public bool IsAllEmployees { get; set; }
        public bool IsChosenDeppartment { get; set; }
        public string ChosenDepartmentName { get; set; }
        public string ChosenDepartmentID { get; set; }
        public bool IsAnnoucement { get; set; }
        public string TemplateURL { get; set; }
        public string TemplateName { get; set; }

        public RemindBirthDaySetting() { }
    }
}
