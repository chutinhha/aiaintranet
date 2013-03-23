using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Report
{
    public class MySampleReport : IReportBase
    {
        [PlaceHolder("FULLNAME", PlaceHolderType.NonRecursive)]
        public string SampleProperty { get; set; }

        [PlaceHolder("Email", PlaceHolderType.NonRecursive)]
        public string SampleProperty2 { get; set; }


        [PlaceHolder("Message", PlaceHolderType.NonRecursive)]
        public string Message { get; set; }

        [PlaceHolder("DATE", PlaceHolderType.NonRecursive)]
        public string Date { get; set; }



    }
}
