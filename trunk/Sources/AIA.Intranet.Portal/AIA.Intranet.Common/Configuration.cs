using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Common.Services;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using System.Reflection;

namespace AIA.Intranet.Common
{
    [System.AttributeUsage(System.AttributeTargets.Property |
                      System.AttributeTargets.Struct,
                      AllowMultiple = true)  // multiuse attribute
    ]
    
    public class ConfigAttribute : Attribute
    {
        public string Key{ get; set; }
        public bool Html { get; set; }
        public object Default { get; set; }
        
    }

    public class Configuration
    {
        private static Configuration config;

        public static Configuration GetInstance()
        {
            if (config == null) {
                config = new Configuration();
                config.Refresh();
            }
            return config;
            
        }
        public static string GetConfiguration(string key, SPWeb web)
        {
            var item = ConfiguratioinService.GetItemByKey(key, web);
            if (item == null) return string.Empty;
            return item.Value;
            
        }
        
        [Config(Key="ABC")]
        public  string Sample { get; set; }

        [Config(Key = "WorkTime")]
        public string WorkTime { get; set; }

        public  void Refresh()
        {
            Refresh(null);
        }
        public void Refresh(SPWeb web)
        {
            var collection = ConfiguratioinService.GetAllItems(web);
            
            Type type = typeof(Configuration); // MyClass is static class with static properties
            foreach (var p in type.GetProperties())
            {
                ConfigAttribute config = (ConfigAttribute)p.GetCustomAttributes(typeof(ConfigAttribute), true).FirstOrDefault();
                string key = p.Name;
                if(config != null)
                    key = config.Key;

                var item = collection.FirstOrDefault(x=>x.Title == key);

                if (item != null)
                {
                    Object objValue = Convert.ChangeType(((ConfigAttribute)config).Html ? item.HtmlValue : item.Value, p.PropertyType);
                    p.SetValue(this, objValue, null);
                }
                else
                {
                    if (config != null)
                    {
                        p.SetValue(this, config.Default, null);
                    }
                }
            }
        }


        public static Configuration GetInstance(SPWeb web)
        {
            if (config == null)
            {
                config = new Configuration();
                config.Refresh(web);
            }
            return config;
        }
        [Config(Key = "Online Refresh Interval", Default=5)]
        public double OnlineRefreshInterval { get; set; }
    }
}

