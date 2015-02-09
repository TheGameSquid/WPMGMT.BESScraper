using System;
using System.Configuration;
using System.ComponentModel;

namespace WPMGMT.BESScraper
{
    public static class AppSettings
    {
        public static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(appSetting))
            {
                throw new Exception(String.Format("Key {0} was not found", key));
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }
}
