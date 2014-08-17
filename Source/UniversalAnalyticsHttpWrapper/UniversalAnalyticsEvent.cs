using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// An object which represents a Universal Analytics Event that will be pushed via the Measurement Protocol. Expects
    /// an App Setting for 'UniversalAnalytics.Version' and 'UniversalAnalytics.TrackingId' in the config. 
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#events
    /// for more details on the primary fields for an event.
    /// </summary>
    /// <exception cref="UniversalAnalyticsHttpWrapper.Exceptions.ConfigEntryMissingException">Thrown when
    /// one of the required config attributes are missing.</exception>
    public class UniversalAnalyticsEvent : UniversalAnalyticsHttpWrapper.IUniversalAnalyticsEvent
    {
        /// <summary>
        /// This key is required in the .config. The Version will only change when the Measurement Protocol
        /// has breaking changes. If that happens, the code in this package may need to be updated.
        /// </summary>
        public const string APP_KEY_UNIVERSAL_ANALYTICS_VERSION = "UniversalAnalytics.Version";
        /// <summary>
        /// This key is required in the .config. Find this value from your Universal Analytics property that was set up on
        /// www.google.com/analytics/
        /// </summary>
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

        /// <summary>
        /// This constructor expects an App Setting for 'UniversalAnalytics.Version' and 'UniversalAnalytics.TrackingId' 
        /// in the config.
        /// </summary>
        /// <param name="anonymousClientId">Required. Anonymous client id for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.</param>
        /// <param name="eventCategory">Required. The event category for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.</param>
        /// <param name="eventAction">Required. The event action for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.</param>
        /// <param name="eventLabel">Optional. The event label for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.</param>
        /// <param name="eventValue">Optional. The event value for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.</param>
        /// <exception cref="UniversalAnalyticsHttpWrapper.Exceptions.ConfigEntryMissingException">Thrown when
        /// one of the required config attributes are missing.</exception>
        /// <exception cref="System.ArgumentException">Thrown when one of the required fields are null or whitespace.</exception>
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

        internal UniversalAnalyticsEvent(
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

        /// <summary>
        /// Gets the measurement protocol version. This is pulled from the .config when the object is constructed.
        /// </summary>
        public string MeasurementProtocolVersion { get { return this.measurementProtocolVersion; } }
        /// <summary>
        /// Gets the tracking id for the Universal Analytics property. This is pulled from the .config when the object is constructed.
        /// </summary>
        public string TrackingId { get { return this.trackingId; } }
        /// <summary>
        /// Gets the anonymousClientId that was passed in when the object was constructed.
        /// </summary>
        public string AnonymousClientId { get { return this.anonymousClientId; } }
        /// <summary>
        /// Gets the eventCategory that was passed in when the object was constructed.
        /// </summary>
        public string EventCategory { get { return this.eventCategory; } }
        /// <summary>
        /// Gets the eventAction that was passed in when the object was constructed.
        /// </summary>
        public string EventAction { get { return this.eventAction; } }
        /// <summary>
        /// Gets the eventLabel that was passed in when the object was constructed.
        /// </summary>
        public string EventLabel { get { return this.eventLabel; } }
        /// <summary>
        /// Gets the eventValue that was passed in when the object was constructed.
        /// </summary>
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
