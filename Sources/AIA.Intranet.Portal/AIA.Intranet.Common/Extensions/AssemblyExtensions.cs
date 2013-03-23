using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace AIA.Intranet.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetResourceTextFile(this Assembly Assem, string filename)
        {
            string result = string.Empty;            
            using (Stream stream = Assem.GetManifestResourceStream(filename))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
