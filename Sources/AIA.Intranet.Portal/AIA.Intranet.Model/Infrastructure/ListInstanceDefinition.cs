using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    public class ListIntanceDefinition
    {
        public string Title { get; set; }
        public string FeatureId { get; set; }
        public int TemplateId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool ShowOnQuickLaunch { get; set; }
        public bool Hidden { get; set; }
        public bool RootWebOnly { get; set; }
        public List<string> ContentTypes { get; set; }

        public string TemplateName { get; set; }
    }
}
