using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class ApprovalWFApprover
    {

        public ApprovalWFApprover()
        {
            SpecificUserGroup = new List<string>();
            ManagerApprove = false;
            UpdateProperties = new List<string>();
            TaskEvents = new TaskEventSettings();
        }
        public TaskEventSettings TaskEvents { get; set; }
        public string ApprovalLevelName { get; set; }

        public string ColumnName { get; set; } //get Approver(s) from a column in list
        public List<string> SpecificUserGroup { get; set; } // get Approver(s) from PeoplePicker
        public bool ManagerApprove { get; set; } // get Approver by select the direct manager of current user starting workflow

        public bool ExpendGroup { get; set; }
        public string TaskSequenceType { get; set; }
        public string DueDate { get; set; }
        public double DurationPerTask { get; set; }
        public bool EnableEmail { get; set; }
        //public string EmailTemplate { get; set; }
        //public string TemplateName { get; set; }
        public EmailTemplateSettings EmailTemplate { get; set; }
        public bool AllowChangeMessage { get; set; }
        public string TaskInstruction { get; set; }
        public string TaskContenType { get; set; }
        public string TaskTitle { get; set; }

        public bool EnableChangeApprovers { get; set; }

        public bool AppendTitle { get; set; }

        public List<string> UpdateProperties { get; set; }
    }
}
