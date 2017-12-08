using System;
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

        /// <summary>
        /// This is the current Google collection URI for version 1 of the measurement protocol
        /// </summary>
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("https://www.google-analytics.com/collect");
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
                string postData = this._postDataBuilder.BuildPostDataString(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent);
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
                var postData = this._postDataBuilder.BuildPostDataCollection(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent);
                await this._googleDataSender.SendDataAsync(GOOGLE_COLLECTION_URI, postData);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }
    }
}
