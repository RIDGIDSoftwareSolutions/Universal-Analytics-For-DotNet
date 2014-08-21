using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Class for making instances of IUniversalAnalyticsEvent objects
    /// </summary>
    public class UniversalAnalyticsEventFactory : IUniversalAnalyticsEventFactory
    {
        /// <summary>
        /// This key is required in the .config. Find this value from your Universal Analytics property that was set up on
        /// www.google.com/analytics/
        /// </summary>
        public const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";

        private IConfigurationManager configurationManager;

        /// <summary>
        /// Default constructor. Could be a singleton but this is easier for the average developer to consume
        /// </summary>
        public UniversalAnalyticsEventFactory()
        {
            this.configurationManager = new ConfigurationManager();
        }

        internal UniversalAnalyticsEventFactory(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// This constructor expects an App Setting for 'UniversalAnalytics.TrackingId' 
        /// in the config. UniversalAnalytics.TrackingId must be a Universal Analytics Web Property.
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
        /// <exception cref="System.Web.HttpException">Thrown when the HttpRequest that's posted to Google returns something
        /// other than a 200 OK response.</exception>
        public IUniversalAnalyticsEvent MakeUniversalAnalyticsEvent(
            string anonymousClientId, 
            string eventCategory, 
            string eventAction, 
            string eventLabel = null, 
            string eventValue = null) 
        {
            string trackingId = RetrieveAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            return new UniversalAnalyticsEvent(
                trackingId,
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);
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
