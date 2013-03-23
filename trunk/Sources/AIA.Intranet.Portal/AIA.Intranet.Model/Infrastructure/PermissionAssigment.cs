using System;
using System.Collections.Generic;

namespace AIA.Intranet.Model.Security
{
    [Serializable]
    public class PermissionAssigment
    {
        public List<string> Members { get; set; }
        public string PermissionLevel { get; set; }
        public List<string> FieldIds { get; set; }
    }
}
