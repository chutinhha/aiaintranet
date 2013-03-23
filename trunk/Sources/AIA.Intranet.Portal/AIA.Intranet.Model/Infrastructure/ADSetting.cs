using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Infrastructure.Models
{
    public class ADSetting
    {
        public string OUListUrl { get; set; }
        public string OUField { get; set; }
        public string OUValue { get; set; }
        public bool IsImpersonate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //public string Domain { get; set; }

        public string LDAP { get; set; }
    }
}
