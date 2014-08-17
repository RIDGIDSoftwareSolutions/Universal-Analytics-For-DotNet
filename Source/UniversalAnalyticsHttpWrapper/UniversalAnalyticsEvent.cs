using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    public class UniversalAnalyticsEvent : UniversalAnalyticsHttpWrapper.IUniversalAnalyticsEvent
    {
        public const string APP_KEY_UNIVERSAL_ANALYTICS_VERSION = "UniversalAnalytics.Version";
        public const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";

        //TODO better to provide a constructor overload or to just use setter injection?
        internal IConfigurationManager configurationManager;

        private string measurementProtocolVersion;
        private string trackingId;
        private string anonymousClientId;
        private string eventCategory;
        private string eventAction;
        private string eventLabel;
        private string eventValue;

        public UniversalAnalyticsEvent(
            string anonymousClientId,
            string eventCategory,
            string eventAction,
            string eventLabel = null,
            string eventValue = null) 
                : this(
                    new ConfigurationManager(), 
                    anonymousClientId, 
                    eventCategory, 
                    eventAction, 
                    eventLabel, 
            eventValue)
        {
           
        }

        public UniversalAnalyticsEvent(
            IConfigurationManager configurationManager,
            string anonymousClientId,
            string eventCategory,
            string eventAction,
            string eventLabel = null,
            string eventValue = null)
        {
            this.configurationManager = configurationManager;
            this.anonymousClientId = anonymousClientId;
            this.eventCategory = eventCategory;
            this.eventAction = eventAction;
            this.eventLabel = eventLabel;
            this.eventValue = eventValue;

            ValidateRequiredFields();

            string version = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_VERSION);
            this.measurementProtocolVersion = version;

            string trackingId = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);
            this.trackingId = trackingId;
        }

        public string MeasurementProtocolVersion { get { return this.measurementProtocolVersion; } }
        public string TrackingId { get { return this.trackingId; } }
        public string AnonymousClientId { get { return this.anonymousClientId; } }
        public string EventCategory { get { return this.eventCategory; } }
        public string EventAction { get { return this.eventAction; } }
        public string EventLabel { get { return this.eventLabel; } }
        public string EventValue { get { return this.eventValue; } }

        private void ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(this.anonymousClientId))
            {
                throw new ArgumentException("analyticsEvent.AnonymousClientId");
            }

            if (string.IsNullOrWhiteSpace(this.eventCategory))
            {
                throw new ArgumentException("analyticsEvent.Category");
            }

            if (string.IsNullOrWhiteSpace(this.eventAction))
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
