using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Model;
//using AIA.Intranet.Model.DocuSign;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
	public class TaskEventHandlerParameter
	{
        public SPWorkflowActivationProperties WorkflowProperties { get; set; }
        public int TaskId { get; set; }
        public TaskEventSettings EventSettings { get; set; }
        //public ESignEventMetadata ESignMetadata { get; set; }
        public List<NameValue> Variables { get; set; }
        public string DestinationListUrl { get; set; }
        public int DestinationItemId { get; set; }
	}
}
