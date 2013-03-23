using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Search
{
    [Serializable]
    public class Criteria
    {
        public string FieldId { get; set; }
        public Operators Operator { get; set; }
        public string Value { get; set; }
    }
}
