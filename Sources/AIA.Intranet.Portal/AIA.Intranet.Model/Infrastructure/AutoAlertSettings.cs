using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class AutoAlertSettings
    {
        public bool Enable { get; set; }
        public bool SendImmediate { get; set; }
        public bool SendAfter { get; set; }
        public int DelayMinutes { get; set; }
        public TriggerTypes ExcuteTriggerType { get; set; }
        public bool AllowInternalUser { get; set; }
        public List<string> InternalUsers { get; set; }
        public bool AllowColumn { get; set; }
        public List<Guid> Columns { get; set; }
        public bool AllowOwner { get; set; }
        public string EmailTemplate { get; set; }
        public string TemplateName { get; set; }
    }
    public enum TriggerTypes
    {
        New,
        Change,

    }
}
