using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    public class WebLeftMenuDefinition
    {
        public string WebUrl { get; set; }

        [XmlElement("LeftMenus")]
        public List<LeftMenuDefinition> LeftMenus { get; set; }

        public WebLeftMenuDefinition()
        {
            LeftMenus = new List<LeftMenuDefinition>();
        }
    }

    [Serializable]
    public class LeftMenuDefinition
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string OrderNo { get; set; }
        public bool Active { get; set; }
        public string Group { get; set; }

        public void CreateMenu()
        {
            
        }
    }
}
