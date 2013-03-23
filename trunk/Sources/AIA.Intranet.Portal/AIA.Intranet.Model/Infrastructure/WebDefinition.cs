using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace AIA.Intranet.Model.Infrastructure
{

    [Serializable]
    [XmlRoot("Web")]
    
    public class WebDefinition
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [XmlArray("Webs")]
        //[XmlArrayItem("Web")]
        
        public WebDefinitionCollection SubSites { get; set; }

         [XmlArray("Features")]
         [XmlArrayItem("Feature")]
        public List<FeatureDefinition> Features { get; set; }

         [XmlArray("Lists")]
         [XmlArrayItem("List")]
        public List<ListIntanceDefinition> Lists { get; set; }

        public WebDefinition()
        {
            
            SubSites = new WebDefinitionCollection();
            Features = new List<FeatureDefinition>();
            Lists = new List<ListIntanceDefinition>();
        }

        public string Template { get; set; }

        public bool Overwrite { get; set; }
    }

    [XmlRoot("Webs")]
    
    public class WebDefinitionCollection : List<WebDefinition>
    {
        public WebDefinitionCollection()
        {
            
        }
    }
}
