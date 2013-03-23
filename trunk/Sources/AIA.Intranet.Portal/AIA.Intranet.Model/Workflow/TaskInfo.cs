using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class TaskInfo
    {
        public string Approver { get; set; }

        public string MessageTitle { get; set; }

        public string Message { get; set; }

        public bool MailEnable { get; set; }

        public string TaskContentType { get; set; }

        public string TaskTitle { get; set; }

        public string TaskInstruction { get; set; }
        public DateTime  DueDate { get; set; }

        public int TaskDuration { get; set; }

        public bool AppendTitle { get; set; }

        public List<string> UpdatedProperties { get; set; }

        public TaskEventSettings TaskEvents { get; set; }

    }
}
