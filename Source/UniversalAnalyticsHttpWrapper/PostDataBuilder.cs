using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    public class PostDataBuilder : IPostDataBuilder
    {
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_VERSION = "UniversalAnalytics.Version";
        internal const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";
        public const string PARAMETER_KEY_VERSION = "v";
        public const string PARAMETER_KEY_TRACKING_ID = "tid";
        public const string PARAMETER_KEY_ANONYMOUS_CLIENT_ID = "cid";
        public const string PARAMETER_KEY_HIT_TYPE = "t";
        public const string PARAMETER_KEY_EVENT_CATEGORY = "ec";
        public const string PARAMETER_KEY_EVENT_ACTION = "ea";
        public const string PARAMETER_KEY_EVENT_LABEL = "el";
        public const string PARAMETER_KEY_EVENT_VALUE = "ev";
        public const string HIT_TYPE_EVENT = "event";

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

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection[PARAMETER_KEY_VERSION] = version;
            nameValueCollection[PARAMETER_KEY_TRACKING_ID] = trackingId;
            nameValueCollection[PARAMETER_KEY_ANONYMOUS_CLIENT_ID] = analyticsEvent.AnonymousClientId;
            nameValueCollection[PARAMETER_KEY_HIT_TYPE] = HitTypeEnum.@event.ToString();
            nameValueCollection[PARAMETER_KEY_EVENT_ACTION] = analyticsEvent.EventAction;
            nameValueCollection[PARAMETER_KEY_EVENT_CATEGORY] = analyticsEvent.EventCategory;

            if(analyticsEvent.EventLabel != null)
            {
                nameValueCollection[PARAMETER_KEY_EVENT_LABEL] = analyticsEvent.EventLabel;
            }

            if(analyticsEvent.EventValue != null)
            {
                nameValueCollection[PARAMETER_KEY_EVENT_VALUE] = analyticsEvent.EventValue;
            }

            string postData = nameValueCollection.ToString();

            return postData;
        }

        private static void ValidateRequiredFields(UniversalAnalyticsEvent analyticsEvent)
        {
            if (string.IsNullOrWhiteSpace(analyticsEvent.AnonymousClientId))
            {
                throw new ArgumentException("analyticsEvent.AnonymousClientId");
            }

            if (string.IsNullOrWhiteSpace(analyticsEvent.EventCategory))
            {
                throw new ArgumentException("analyticsEvent.Category");
            }

            if (string.IsNullOrWhiteSpace(analyticsEvent.EventAction))
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

        public enum HitTypeEnum
        {
            @event
        }
    }
}
