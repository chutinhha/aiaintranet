using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class ApprovalWFInitiationData
    {
        public ApprovalWFInitiationData()
        {
            ApprovalLevels = new List<ApprovalLevelInfo>();
        }

        public List<ApprovalLevelInfo> ApprovalLevels { get; set; }
    }
}
