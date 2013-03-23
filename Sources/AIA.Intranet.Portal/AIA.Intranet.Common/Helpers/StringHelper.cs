using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Common.Helpers
{
    public class StringHelper
    {
        public static string FirstWord(string s)
        {
            int index = s.IndexOf(' ');
            if (index > 0) return s.Substring(0, index);
            return s;
        }
    }
}
