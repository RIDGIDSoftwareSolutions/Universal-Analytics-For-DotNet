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
        private IPostDataBuilder postDataBuilder;
        private IGoogleDataSender googleDataSender;
        /// <summary>
        /// This is the current Google collection URI for the measurement protocol, as of 2014-08-17
        /// </summary>
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("http://www.google-analytics.com/collect");

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
        /// Pushes an event up to the Universal Analytics property specified in the .config file.
        /// </summary>
        /// <param name="analyticsEvent"></param>
        public void TrackEvent(IUniversalAnalyticsEvent analyticsEvent)
        {
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
        }
    }
}
