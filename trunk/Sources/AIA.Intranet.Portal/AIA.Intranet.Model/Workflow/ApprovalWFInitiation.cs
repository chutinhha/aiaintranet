using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class ApprovalLevelInfo
    {
        public ApprovalLevelInfo()
        {
            SpecificUserGroup = new List<string>();
        }

        public List<string> UpdatedProperties { get; set; }
        public List<string> SpecificUserGroup { get; set; }
        public DateTime DueDate { get; set; }
        public int DurationPerTask { get; set; }
        public string Message { get; set; }

        public bool EnableEmail { get; set; }
        public bool ExpendGroup { get; set; }
        public string TaskSequenceType { get; set; }
        public EmailTemplateSettings EmailTemplate { get; set; }
        public bool AllowChangeMessage { get; set; }

        public string TaskContenType { get; set; }
        public string TaskTitle { get; set; }
        public bool AppendTitle { get; set; }
        public string MessageTitle { get; set; }
        public string TaskInstruction { get; set; }

        public TaskFormOption FormOption { get; set; }
        public bool EndAtFirstRejection { get; set; }

        public string LevelName { get; set; }


        public TaskEventSettings TaskEvents { get; set; }

        public ApprovalWFApprover ApproverSetting { get; set; }
    }
}
