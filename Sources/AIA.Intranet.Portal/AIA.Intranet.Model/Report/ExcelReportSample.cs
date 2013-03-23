using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Report
{
    public class ExcelReportSample : IReportBase
    {
        [Cell(Index="A2")]
        public string MyProperty { get; set; }
    }
}
