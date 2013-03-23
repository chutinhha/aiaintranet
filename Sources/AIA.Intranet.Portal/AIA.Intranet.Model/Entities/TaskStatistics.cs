using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Entities
{
    [Serializable]

    public class TaskStatistics
    {
        public int Personal { get; set; }
        public int Projects { get; set; }
        public int Departments { get; set; }

        public int Rate_1 { get; set; }
        public int Rate_2 { get; set; }
        public int Rate_3 { get; set; }
        public int Rate_4 { get; set; }
        public int Rate_5 { get; set; }

        public int Assigned { get; set; }
        public int InProcessing { get; set; }
        public int Completed { get; set; }
        public int Canceled { get; set; }
        public int OnHold { get; set; }



    }
}
