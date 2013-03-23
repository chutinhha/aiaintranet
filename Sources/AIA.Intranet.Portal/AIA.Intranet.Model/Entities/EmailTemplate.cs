using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    public class EmailTemplate : BaseEntity
    {

        public string Subject { get; set; }
        public string Body { get; set; }

        [Field(FieldName = "SendAsPlainText")]
        public Boolean SendAsPlainText { get; set; }

        public EmailTemplate(SPListItem item) : base(item){}
        
    }
}
