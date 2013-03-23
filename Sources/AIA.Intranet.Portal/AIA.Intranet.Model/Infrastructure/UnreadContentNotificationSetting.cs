using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    public class UnreadContentNotificationSetting : SettingBase
    {
        public bool Enable { get; set; }

        public bool EnableEmail { get; set; }

        public bool EnableCreateUnreadTask { get; set; }
        
        

        public EmailTemplateSettings Template
        {
            get;
            set;
        }

        public string TitleFormula { get; set; }

        public UnreadContentNotificationSetting()
        {
            Template = new EmailTemplateSettings();
        }


        
    }
}
