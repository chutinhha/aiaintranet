using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    [XmlRoot("List")]
    public class ListInstancePermission
    {
        public string ListUrl { get; set; }
        public bool BrokenInheritance { get; set; }

        public List<Assignement> Assignements { get; set; }

        
       /// public List<UserAssignment> Users { get; set; }
    }

    public class ListInstancePermissionCollection : List<ListInstancePermission>
    {
        
        
    }
}
