using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPWebApplicationExtensions
    {
        public static string GetFeaturePropertyValue(this SPWebApplication webApp, string featureId, string key)
        {
            var theFeature = webApp.Features[new Guid(featureId)];
            if (theFeature != null && theFeature.Properties[key] != null)
                return theFeature.Properties[key].Value;
            return string.Empty;
        }

        public static string GetCustomProperty(this SPWebApplication webApplication, string key)
        {
            String propValue = String.Empty;
            if (webApplication.Properties.ContainsKey(key))
            {
                propValue = (string)webApplication.Properties[key];
            }

            return propValue;
        }

        public static void SetCustomProperty(this SPWebApplication webApplication, string key, string value)
        {
            if (webApplication.Properties.ContainsKey(key))
            {
                webApplication.Properties[key] = value;
            }
            else
            {
                webApplication.Properties.Add(key, value);
            }
            webApplication.Update();
            //webApplication.Properties..Update();
        }
    }
}
