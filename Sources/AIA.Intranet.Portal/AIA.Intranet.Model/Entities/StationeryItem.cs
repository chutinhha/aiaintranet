using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]
    public class StationeryItem
    {
        public string Department { get; set; }
        public int Projects { get; set; }
        public int Departments { get; set; }
    }
}
