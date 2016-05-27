using System.Configuration;

namespace JetermUntility
{
    public class ConfigHelper
    {
        public static string GetAppSettingValue(string key)
        {
            try
            {
                var keyValue = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(keyValue))
                {
                    return string.Empty;
                }

                return keyValue;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetAppSettingValue(string key, string defaultValue)
        {
            try
            {
                var keyValue = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(keyValue))
                {
                    return defaultValue;
                }

                return keyValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetConnectionSettingValue(string key)
        {
            try
            {
                var keyValue = ConfigurationManager.ConnectionStrings[key];
                if (keyValue == null || string.IsNullOrEmpty(keyValue.ConnectionString))
                {
                    return string.Empty;
                }
                return keyValue.ConnectionString;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
