using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    public class LeftMenuDefinition
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string OrderNo { get; set; }
        public bool Active { get; set; }
        public string MenuKeywords { get; set; }
    }

    [Serializable]
    [XmlRoot("Web")]

    public class WebMenuDefinition
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [XmlArray("LeftMenus")]
        [XmlArrayItem("LeftMenu")]
        public List<LeftMenuDefinition> Features { get; set; }

        public WebMenuDefinition()
        {
            Features = new List<LeftMenuDefinition>();
        }
    }

    [XmlRoot("Webs")]
    public class WebMenuDefinitionCollection : List<WebMenuDefinition>
    {
        public WebMenuDefinitionCollection()
        {

        }
    }
}
