using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class TaskFormOption
    {
        public TaskFormOption()
        {
            EnableApprove = true;
            EnableReject = true;
        }
        public string ApproveLabel { get; set; }
        public string RejectLabel { get; set; }
        public string RequestInformationLabel { get; set; }
        public string RequestChangeLabel { get; set; }
        public string OnHoldLabel { get; set; }
        public string ReassignLabel { get; set; }
        
        public bool EnableApprove { get; set; }
        public bool EnableReject { get; set; }
        public bool EnableHoldOn { get; set; }
        public bool EnableRequestChange { get; set; }
        public bool EnableRequestInf { get; set; }
        public bool EnableReassign { get; set; }

    }
}
