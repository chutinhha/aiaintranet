using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class AutoCreationSettings : SettingBase
    {
        public bool EnableCreateList { get; set; }
        public bool EnableCreateWeb { get; set; }
        public ListIntanceDefinition ListDefinition { get; set; }
        public WebDefinition WebDefinition { get; set; }

        public bool RunOnChanged { get; set; }

        public bool RunOnCreated { get; set; }

        public bool RunOnFirstCheckIn { get; set; }


        public string UrlFieldName { get; set; }

        public bool EnableNavigationUpdate { get; set; }
        public NavigationUpdateProperties NavigationUpdate { get; set; } 
    }
}
