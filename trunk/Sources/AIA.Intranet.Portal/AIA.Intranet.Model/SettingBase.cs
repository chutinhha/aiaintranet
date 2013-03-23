using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Model
{
    [XmlInclude (typeof(AutoCreationSettings))]
    [XmlInclude(typeof(NotificationSettings))]
    [XmlInclude(typeof(UnreadContentNotificationSetting))]
    [XmlInclude(typeof(NotificationSettingsCollection))]
    [Serializable]

    public class SettingBase
    {
        public string Version { get; set; }

        public SettingBase()
        {
            Version = "I-Office v1.0.0.0";
        }
    }
}
