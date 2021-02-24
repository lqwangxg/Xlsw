using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace XlsWxg
{
    public class Config
    {
        public static string GetAppSettingValue(string key)
        {
            return GetAppSettingValue2(key, string.Empty);
        }
        public static string GetAppSettingValue2(string key, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public static Encoding Encoding
        {
            get
            {
                string encoding = GetAppSettingValue2("encoding", "UTF-8");
                return Encoding.GetEncoding(encoding);
            }
        }
    }
}
