using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Xml.Serialization;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    
    public class Assignement
    {
        /// <summary>
        /// This is group or login name
        /// </summary>
        public string Name { get; set; }

        public  List<SPBasePermissions> Permissions { get; set; }
        public List<SPRoleType> RoleDefinitions { get; set; }

            public Assignement() {
                Permissions = new List<SPBasePermissions>();
                RoleDefinitions = new List<SPRoleType>();
            }
    }
}
