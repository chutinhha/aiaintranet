using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Model
{
    [XmlInclude (typeof(AutoCreationSettings))]
    [Serializable]

    public class SettingBase
    {
        public string Version { get; set; }

        public SettingBase()
        {
            Version = "AIA Intranet Portal v1.0.0.0";
        }
    }
}
