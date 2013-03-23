using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
	public class SendRelatedDocumentsSettings
	{
        public string EmailTemplateListUrl {get;set;}
        public string EmailTemplateName { get; set; }
        public int MaximunAttachments { get; set; }
        public int AttachmentSizeLimit { get; set; }
	}
}
