using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Report
{

    [System.AttributeUsage(System.AttributeTargets.Property |
                           System.AttributeTargets.Struct,
                           AllowMultiple = true)  // multiuse attribute
    ]

    public class PlaceHolderAttribute : Attribute
    {
        public string Name { get; set; }
        public PlaceHolderType Type{ get; set; }
        public PlaceHolderAttribute(){}
        public PlaceHolderAttribute(string name, PlaceHolderType type)
        {
            Name = name;
            Type = type;
        }
    }
}
