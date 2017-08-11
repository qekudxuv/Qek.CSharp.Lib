using System;
using System.Configuration;

namespace Qek.Common
{
    public static class ConfigHelper
    {
        public static string GetAppSetting(string key, ENV? env = null)
        {
            string item = ConfigurationManager.AppSettings[GetKey(key, env)];
            if (string.IsNullOrEmpty(item))
            {
                throw new Exception(string.Format("AppSetting {0} is not set!", key));
            }
            return item;
        }

        public static T GetAppSetting<T>(string key, ENV? env = null)
        {
            T t;
            try
            {
                string item = ConfigurationManager.AppSettings[GetKey(key, env)];
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), item);
                }

                t = (T)Convert.ChangeType(item, typeof(T));
            }
            catch
            {
                throw new Exception(string.Format("GetAppSetting error: inputKey[{0}], outputType[{1}]", key, typeof(T).Name));
            }
            return t;
        }

        public static string GetAppSettingOrDefault(string key, ENV? env = null, string defaultValue = "")
        {
            string item = ConfigurationManager.AppSettings[GetKey(key, env)];
            if (string.IsNullOrEmpty(item))
            {
                item = defaultValue;
            }
            return item;
        }

        public static T GetAppSettingOrDefault<T>(string key, ENV? env = null, T defaultValue = default(T))
        {
            T t;
            try
            {
                string item = ConfigurationManager.AppSettings[GetKey(key, env)];
                t = (T)Convert.ChangeType(item, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
            return t;
        }

        public static string GetConnectionString(string key, ENV? env = null)
        {
            ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[GetKey(key, env)];
            if (item == null)
            {
                throw new Exception(string.Format("ConnectionString {0} is not set!", key));
            }
            return item.ConnectionString;
        }

        private static string GetKey(string key, ENV? env = null)
        {
            if (env.HasValue)
            {
                key = string.Format("{0}_{1}", env.ToString(), key);
            }
            return key;
        }
    }
}

