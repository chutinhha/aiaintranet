using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AIA.Intranet.Common.Helpers
{
    public class RequestHelper
    {
        public static string GetString(string key, string defaultValue)
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = defaultValue;
            if (request != null && request[key] != null)
            {
                result = request[key];
            }
            return result;
        }
    }
}
