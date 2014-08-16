using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Configuration.Abstractions;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    public class EventTracker : IEventTracker
    {
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_VERSION = "UniversalAnalytics.Version";
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";

        private IConfigurationManager configurationManager;
        private IPostDataBuilder postDataBuilder;
        private IGoogleDataSender googleDataSender;
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("http://www.google-analytics.com/collect");

        public EventTracker(IConfigurationManager configurationManager, IPostDataBuilder postDataBuilder, IGoogleDataSender googleDataSender)
        {
            this.configurationManager = configurationManager;
            this.postDataBuilder = postDataBuilder;
            this.googleDataSender = googleDataSender;
        }

        public void TrackEvent(UniversalAnalyticsEvent analyticsEvent)
        {
            ValidateRequiredFields(analyticsEvent);

            string version = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_VERSION);    
            string trackingId = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            string postData = postDataBuilder.BuildPostDataString(version, trackingId, analyticsEvent);

            googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
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
