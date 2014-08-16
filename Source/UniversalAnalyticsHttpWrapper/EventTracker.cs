using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Configuration.Abstractions;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper
{
    public class EventTracker : IEventTracker
    {
        private IPostDataBuilder postDataBuilder;
        private IGoogleDataSender googleDataSender;
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("http://www.google-analytics.com/collect");

        public EventTracker(IPostDataBuilder postDataBuilder, IGoogleDataSender googleDataSender)
        {
            this.postDataBuilder = postDataBuilder;
            this.googleDataSender = googleDataSender;
        }

        public void TrackEvent(UniversalAnalyticsEvent analyticsEvent)
        {
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
        }
    }
}
