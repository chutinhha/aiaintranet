using System;
using System.Collections.Generic;
using AIA.Intranet.Model.Security;

namespace AIA.Intranet.Model.Security
{
    [Serializable]
    public class SecuritySettings
    {

        public SecuritySettings()
        {
            Rules = new List<Rule>();
        }

        public List<Rule> Rules { get; set; }
        
    }
}
