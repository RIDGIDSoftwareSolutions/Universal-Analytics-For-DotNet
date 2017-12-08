using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace UniversalAnalyticsHttpWrapper
{
    internal class PostDataBuilder : IPostDataBuilder
    {
        internal const string PARAMETER_KEY_VERSION = "v";
        internal const string PARAMETER_KEY_TRACKING_ID = "tid";
        internal const string PARAMETER_KEY_ANONYMOUS_CLIENT_ID = "cid";
        internal const string PARAMETER_KEY_USER_ID = "uid";
        internal const string PARAMETER_KEY_HIT_TYPE = "t";
        internal const string PARAMETER_KEY_EVENT_CATEGORY = "ec";
        internal const string PARAMETER_KEY_EVENT_ACTION = "ea";
        internal const string PARAMETER_KEY_EVENT_LABEL = "el";
        internal const string PARAMETER_KEY_EVENT_VALUE = "ev";
        internal const string HIT_TYPE_EVENT = "event";

        public string BuildPostDataString(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent)
        {
            var postData = BuildPostData(measurementProtocolVersion, analyticsEvent);
            return postData.ToString();
        }

        public IEnumerable<KeyValuePair<string, string>> BuildPostDataCollection(
            string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent)
        {
            var postData = BuildPostData(measurementProtocolVersion, analyticsEvent);
            var collection = postData.AllKeys.SelectMany(
                postData.GetValues,
                (key, value) => new KeyValuePair<string, string>(key, value));

            return collection;
        }

        internal NameValueCollection BuildPostData(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent)
        {
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection[PARAMETER_KEY_VERSION] = measurementProtocolVersion;
            nameValueCollection[PARAMETER_KEY_TRACKING_ID] = analyticsEvent.TrackingId;

            if (!string.IsNullOrWhiteSpace(analyticsEvent.UserId))
            {
                nameValueCollection[PARAMETER_KEY_USER_ID] = analyticsEvent.UserId;
            }

            if (!string.IsNullOrWhiteSpace(analyticsEvent.AnonymousClientId))
            { 
                nameValueCollection[PARAMETER_KEY_ANONYMOUS_CLIENT_ID] = analyticsEvent.AnonymousClientId;
            }

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

            return nameValueCollection;
        }

        /// <summary>
        /// Represents the different Measurement Protocol hit types supported by this package.
        /// </summary>
        internal enum HitTypeEnum
        {
            @event
        }
    }
}
