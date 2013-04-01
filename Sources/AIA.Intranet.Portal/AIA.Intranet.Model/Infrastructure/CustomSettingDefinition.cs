using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Infrastructure
{
    public class Receiver{
        public string ReceiverClass { get; set; }
        [XmlArray("Types")]
        [XmlArrayItem("Type")]
        public List<SPEventReceiverType> Types { get; set; }

        public string ReceiverAssembly { get; set; }
    }

    [Serializable]
    [XmlInclude(typeof(ContentTypeSettingDefinition))]
    [XmlInclude(typeof(ListSettingDefinition))]
    public class CustomSettingDefinition
    {
        public IOfficeFeatures Feature { get; set; }
        public string Object { get; set; }
        public SettingBase Data { get; set; }
        
        public SettingLevels Level { get; set; }
        public CustomSettingDefinition()
        {
            
        }

        [XmlArray("Receivers")]
        public List<Receiver> Receivers { get; set; }
    }

    [Serializable]
    public class ContentTypeSettingDefinition : CustomSettingDefinition
    {
        public string ContentTypeId { get; set; }
        public bool Inherit { get; set; }
        public ContentTypeSettingDefinition()
        {
            Level = SettingLevels.ContentType;
        }
    }

    [Serializable]
    public class ListSettingDefinition : CustomSettingDefinition
    {
        public string ListUrl { get; set; }
        public string ListId { get; set; }
        public bool Inherit { get; set; }
        public ListSettingDefinition()
        {
            Level = SettingLevels.List;
        }
    }

    [Serializable]
    public enum SettingLevels
    {
        ContentType,
        ListContentType,
        Item,
        Web,
        List,
        Site,

    }
    
}
