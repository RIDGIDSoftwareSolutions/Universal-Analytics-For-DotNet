using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Configuration.Abstractions;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// This is the primary class with which the consumer will work.
    /// </summary>
    public class EventTracker : IEventTracker
    {
        private readonly IPostDataBuilder postDataBuilder;
        private readonly IGoogleDataSender googleDataSender;
        /// <summary>
        /// This is the current Google collection URI for version 1 of the measurement protocol
        /// </summary>
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("http://www.google-analytics.com/collect");
        /// <summary>
        /// This assembly is built to work with this version of the measurement protocol.
        /// </summary>
        public const string MEASUREMENT_PROTOCOL_VERSION = "1";

        /// <summary>
        /// This constructor will create all of the appropriate default implementations.
        /// </summary>
        public EventTracker()
        {
            this.postDataBuilder = new PostDataBuilder();
            this.googleDataSender = new GoogleDataSender();
        }

        /// <summary>
        /// This constructor is only used if you want to inject your own implementations (most likely for unit testing)
        /// </summary>
        /// <param name="postDataBuilder"></param>
        /// <param name="googleDataSender"></param>
        internal EventTracker(IPostDataBuilder postDataBuilder, IGoogleDataSender googleDataSender)
        {
            this.postDataBuilder = postDataBuilder;
            this.googleDataSender = googleDataSender;
        }

        /// <summary>
        /// Pushes an event up to the Universal Analytics web property specified in the .config file.
        /// </summary>
        /// <param name="analyticsEvent">The event to be logged.</param>
        public void TrackEvent(IUniversalAnalyticsEvent analyticsEvent)
        {
            string postData = postDataBuilder.BuildPostDataString(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent);

            googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
        }
    }
}
