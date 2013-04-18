using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    [XmlRoot("ContactSetting")]
    public class ContactSettingItem
    {
        public string Subject { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
    }
}
