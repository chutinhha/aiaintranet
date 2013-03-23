using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    [XmlRoot("Field")]
    public class LookupFieldDefinition
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string Web { get; set; }
        public string List { get; set; }
    }
   [XmlRoot("Fields")]
   //[XmlArrayItem("Field", typeof(LookupFieldDefinition))]
    public class LookupFieldDefinitionCollection : List<LookupFieldDefinition>
    {

    }
}
