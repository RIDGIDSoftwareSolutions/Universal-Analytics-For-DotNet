using System.Configuration;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    internal class ConfigurationManagerWrapper : IConfigurationManager
    {
        public string GetAppSetting(string appKey)
        {
            var configValue = ConfigurationManager.AppSettings[appKey];
            if (string.IsNullOrEmpty(configValue))
            {
                throw new ConfigEntryMissingException(appKey);
            }
            return configValue;
        }
    }
}
