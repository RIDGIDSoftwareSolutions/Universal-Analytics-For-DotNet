using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// This is the primary class with which the consumer will work.
    /// </summary>
    public class EventTracker : IEventTracker
    {
        private readonly IPostDataBuilder _postDataBuilder;
        private readonly IGoogleDataSender _googleDataSender;
        private readonly NameValueCollection _customPayload;

        /// <summary>
        /// This is the current Google collection URI for version 1 of the measurement protocol
        /// </summary>
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("https://www.google-analytics.com/collect");

        /// <summary>
        /// This is the current Google collection URI used to validate measurement protocol hits. Data sent to this uri is not registered in GA and as a response you can see json object with validation result
        /// </summary>
        //public static readonly Uri GOOGLE_COLLECTION_URI_DEBUG = new Uri("https://www.google-analytics.com/debug/collect");


        /// <summary>
        /// This assembly is built to work with this version of the measurement protocol.
        /// </summary>
        public const string MEASUREMENT_PROTOCOL_VERSION = "1";

        /// <summary>
        /// This constructor will create all of the appropriate default implementations.
        /// </summary>
        public EventTracker()
        {
            this._postDataBuilder = new PostDataBuilder();
            this._googleDataSender = new GoogleDataSender();
            this._customPayload = new NameValueCollection();
        }

        /// <summary>
        /// This constructor is only used if you want to inject your own implementations (most likely for unit testing)
        /// </summary>
        /// <param name="postDataBuilder"></param>
        /// <param name="googleDataSender"></param>
        internal EventTracker(IPostDataBuilder postDataBuilder, IGoogleDataSender googleDataSender)
        {
            this._postDataBuilder = postDataBuilder;
            this._googleDataSender = googleDataSender;
        }

        /// <summary>
        /// Tracks the event and puts the result in a container object.
        /// </summary>
        /// <param name="analyticsEvent"></param>
        /// <returns>The result of the tracking operation</returns>
        public TrackingResult TrackEvent(IUniversalAnalyticsEvent analyticsEvent)
        { 
            var result = new TrackingResult();
           
            try
            {
                string postData = this._postDataBuilder.BuildPostDataString(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent, _customPayload);
                this._googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }

        /// <summary>
        /// Tracks the event and puts the result in a container object.
        /// </summary>
        /// <param name="analyticsEvent"></param>
        /// <returns>The result of the tracking operation</returns>
        public async Task<TrackingResult> TrackEventAsync(IUniversalAnalyticsEvent analyticsEvent)
        {
            var result = new TrackingResult();

            try
            {
                var postData = this._postDataBuilder.BuildPostDataCollection(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent, _customPayload);
                await this._googleDataSender.SendDataAsync(GOOGLE_COLLECTION_URI, postData);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }

        /// <summary>
        /// Adds "raw" (custom) payload to the hit data.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters for parameters list. For example AddToCustomPayload("aip", "1") to enable IP anonymization.
        /// If parameter was already added, it's value will be replaced with the supplied one
        /// </summary>
        /// <param name="name">Google Analytics Measurement Protocol short parameter name. for ex: aip, ds, qt, etc</param>
        /// <param name="value">Parameter value</param>
        public void AddToCustomPayload(string name, string value)
        {
            foreach (string parameter in _postDataBuilder.SupportedParameters)
            {
                if (parameter == name)
                {
                    throw new ArgumentException(string.Format("Parameter {0} should not be added as a Custom Payload. Use public object properties instead.", name));
                }
            }

            if (_customPayload[name] != null)
            {
                _customPayload[name] = value;
            }
            else
            {
                _customPayload.Add(name, value);
            }
        }
    }
}
