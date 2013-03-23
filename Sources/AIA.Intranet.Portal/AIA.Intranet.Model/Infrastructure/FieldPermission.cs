using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class FieldPermissions
    {
        public List<string> NewGroupPermissions { get; set; }
        public List<string> EditGroupPermissions { get; set; }
        public List<string> ViewGroupPermission { get; set; }


        public List<string> NewUserPermissions { get; set; }
        public List<string> EditUserPermissions { get; set; }
        public List<string> ViewUserPermissions { get; set; }


        public  FieldPermissions() {
            NewGroupPermissions = new List<string>();
            EditGroupPermissions = new List<string>();
            ViewGroupPermission = new List<string>();


            NewUserPermissions = new List<string>();
            EditUserPermissions = new List<string>();
            ViewUserPermissions = new List<string>();


        }

        public Guid FieldId { get; set; }
    }
}
