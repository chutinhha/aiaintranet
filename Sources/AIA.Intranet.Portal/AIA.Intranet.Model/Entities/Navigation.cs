using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Entities
{
    public class Navigation
    {
        public string Name { get; set; }
        public string NavigationUrl { get; set; }
        public int Order { get; set; }
        public string NavigationKey { get; set; }
        public List<Navigation> Childrens { get; set; }

        public string Groups { get; set; }
        public bool IsLeaf { get; set; }
    }
    public enum NavigationNodeTypes
    {
        News,
        Document,
        Company,
        Workflow
    }
}
