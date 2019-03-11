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
        internal const string PARAMETER_KEY_NON_INTERACTION_HIT = "ni";

        internal const string HIT_TYPE_EVENT = "event";


        public string BuildPostDataString(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent)
        {
            var postData = BuildPostData(measurementProtocolVersion, analyticsEvent);
            return postData.ToString();
        }

        public IEnumerable<KeyValuePair<string, string>> BuildPostDataCollection(
            string measurementProtocolVersion, 
            IUniversalAnalyticsEvent analyticsEvent,
            NameValueCollection customPayload = null
            )
        {
            var postData = BuildPostData(measurementProtocolVersion, analyticsEvent, customPayload);

            var collection = postData.AllKeys.SelectMany(
                postData.GetValues,
                (key, value) => new KeyValuePair<string, string>(key, value));

            

            return collection;
        }

        internal NameValueCollection BuildPostData(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent, NameValueCollection customPayload = null)
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

            if (analyticsEvent.NonInteractionHit)
            {
                nameValueCollection[PARAMETER_KEY_NON_INTERACTION_HIT] = "1";
            }

            //adding custom/externally specified parameters
            if (customPayload != null)
            {
                foreach (string parameter in customPayload)
                {
                    if (nameValueCollection[parameter] == null) //do not override supported parameters with user ones
                    {
                        nameValueCollection[parameter] = customPayload[parameter];
                    }
                }
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


        //lazy initialization of supported GA MP parameters list
        private string[] _supportedParameters = null;
        public string[] SupportedParameters
        {
            get
            {
                if (_supportedParameters == null)    //no need to sync
                {
                    _supportedParameters = new string[] {
                        PARAMETER_KEY_VERSION, PARAMETER_KEY_TRACKING_ID, PARAMETER_KEY_ANONYMOUS_CLIENT_ID, PARAMETER_KEY_USER_ID, PARAMETER_KEY_HIT_TYPE,
                        PARAMETER_KEY_EVENT_CATEGORY, PARAMETER_KEY_EVENT_ACTION, PARAMETER_KEY_EVENT_LABEL, PARAMETER_KEY_EVENT_VALUE, PARAMETER_KEY_NON_INTERACTION_HIT
                    };
                }

                return _supportedParameters;
            }
        }


    }
}
