using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    public class PostDataBuilder : IPostDataBuilder
    {
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_VERSION = "UniversalAnalytics.Version";
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";

        private IConfigurationManager configurationManager;

        public PostDataBuilder()
        {
            this.configurationManager = new ConfigurationManager();
        }

        public PostDataBuilder(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public string BuildPostDataString(UniversalAnalyticsEvent analyticsEvent)
        {
            ValidateRequiredFields(analyticsEvent);

            string version = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_VERSION);
            string trackingId = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            throw new NotImplementedException();
        }

        private static void ValidateRequiredFields(UniversalAnalyticsEvent analyticsEvent)
        {
            if (string.IsNullOrWhiteSpace(analyticsEvent.AnonymousClientId))
            {
                throw new ArgumentException("analyticsEvent.AnonymousClientId");
            }

            if (string.IsNullOrWhiteSpace(analyticsEvent.Category))
            {
                throw new ArgumentException("analyticsEvent.Category");
            }

            if (string.IsNullOrWhiteSpace(analyticsEvent.Action))
            {
                throw new ArgumentException("analyticsEvent.Action");
            }
        }

        private string RetrieveAppSetting(string appKey)
        {
            string appSetting = configurationManager.AppSettings[appKey];
            if (appSetting == null)
            {
                throw new ConfigEntryMissingException(appKey);
            }

            return appSetting;
        }
    }
}
