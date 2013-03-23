using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Model.Infrastructure
{
    public class NotificationSettingsCollection: SettingBase{
        public List<NotificationSettings> Settings { get; set; }
        public NotificationSettingsCollection()
        {
            Settings = new List<NotificationSettings>();
        }
    }
    [Serializable]
   public class NotificationSettings 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public EmailTemplateSettings Template { get; set; }

        public bool RunOnChanged { get; set; }

        public bool RunOnCreated { get; set; }

        public bool RunOnFirstCheckIn { get; set; }

        public bool SendToAll { get; set; }

        public bool SendToSpecificUsers { get; set; }

        public bool SendToSelectedColumns { get; set; }
        public List<string> SelectedColumns { get; set; }
        public List<string> SelectedUserOrGroups { get; set; }

        public List<string> CCColumns { get; set; }
        public List<string> CCUserOrGroups { get; set; }
        public List<Criteria> Conditions { get; set; }

        public  NotificationSettings()
        {
            SelectedUserOrGroups = new List<string>();
            SelectedColumns = new List<string>();
            CCColumns = new List<string>();
            CCUserOrGroups = new List<string>();
            Conditions = new List<Criteria>();
        }

        public List<string> Maillists { get; set; }

        public bool SendToMaillist { get; set; }

        public bool EnableCondition { get; set; }
    }
}
