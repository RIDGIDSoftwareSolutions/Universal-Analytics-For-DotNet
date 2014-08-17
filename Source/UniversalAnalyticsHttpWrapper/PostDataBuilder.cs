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
        public const string PARAMETER_KEY_VERSION = "v";
        public const string PARAMETER_KEY_TRACKING_ID = "tid";
        public const string PARAMETER_KEY_ANONYMOUS_CLIENT_ID = "cid";
        public const string PARAMETER_KEY_HIT_TYPE = "t";
        public const string PARAMETER_KEY_EVENT_CATEGORY = "ec";
        public const string PARAMETER_KEY_EVENT_ACTION = "ea";
        public const string PARAMETER_KEY_EVENT_LABEL = "el";
        public const string PARAMETER_KEY_EVENT_VALUE = "ev";
        public const string HIT_TYPE_EVENT = "event";

        public PostDataBuilder()
        {
        }

        public string BuildPostDataString(IUniversalAnalyticsEvent analyticsEvent)
        {
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection[PARAMETER_KEY_VERSION] = analyticsEvent.MeasurementProtocolVersion;
            nameValueCollection[PARAMETER_KEY_TRACKING_ID] = analyticsEvent.TrackingId;
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

            return nameValueCollection.ToString();
        }

        public enum HitTypeEnum
        {
            @event
        }
    }
}
