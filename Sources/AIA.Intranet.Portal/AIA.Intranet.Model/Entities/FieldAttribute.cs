using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Entities
{
     [System.AttributeUsage(System.AttributeTargets.Property |
                         System.AttributeTargets.Struct,
                         AllowMultiple = true)  // multiuse attribute
    ]
   public  class FieldAttribute: Attribute
    {
        public string FieldName { get; set; }
        public string Type { get; set; }
        public bool Ignore { get; set; }

    }
}
