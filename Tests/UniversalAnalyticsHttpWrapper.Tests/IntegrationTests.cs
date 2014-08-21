using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private UniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory(new ConfigurationManager());

        [Test, Ignore("Integration tests.")]
        public void SampleCodeForGitHubReadMeUsingFactoryToGetEventObject()
        {
            //create a new EventTracker
            IEventTracker eventTracker = new EventTracker();
            //create a new event to pass to the event tracker
            IUniversalAnalyticsEvent analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
                //Required. Anonymous client id. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
                "developer",
                //Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                //Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                //Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                //Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10");
            eventTracker.TrackEvent(analyticsEvent);
        }

        [Test, Ignore("Integration tests.")]
        public void SampleCodeForGitHubReadMeManuallyConstructingEventObject()
        {
            //create a new EventTracker
            IEventTracker eventTracker = new EventTracker();
            //create a new event to pass to the event tracker
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
                //Required. The measurement protocol version of the collection service.
                // If you don't want to pass this every time, set the UniversalAnalytics.Version app setting and use the
                // UniversalAnalyticsEventFactory to get instances of this class.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#v for details.
                "1",
                //Required. The universal analytics tracking id for the property 
                // that events will be logged to. If you don't want to pass this every time, set the UniversalAnalytics.TrackingId 
                // app setting and use the UniversalAnalyticsEventFactory to get instances of this class.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#tid for details.
                "UA-52982625-3",
                //Required. Anonymous client id. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
                "developer",
                //Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                //Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                //Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                //Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10");
            eventTracker.TrackEvent(analyticsEvent);
        }

        [Test, Ignore("Integration tests.")]
        public void Send100EventsAsQuicklyAsPossibleAndARateLimitStillWontOccur()
        {
            EventTracker eventTracker = new EventTracker();

            for (int i = 1; i <= 100; i++)
            {
                try
                {
                    IUniversalAnalyticsEvent analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
                         "rate limit test user",
                         "rate limit test category",
                         "rate limit test action",
                         "rate limit test label",
                         i.ToString());
                    eventTracker.TrackEvent(analyticsEvent);
                }catch(Exception e)
                {
                    Assert.Fail("failed after " + i + " transmissions with the following exeption: "+ e.Message);
                }
            }
        }

        [Test, Ignore("Integration tests.")]
        public void SendingEventsFromTwoDifferentUsersShowsUpAsTwoActiveSessionsInUniversalAnalytics()
        {
            //you actually have to watch the analytics property to see this one happening. No assertion necessary.
            EventTracker eventTracker = new EventTracker();

            IUniversalAnalyticsEvent analyticsEventForUser1 = eventFactory.MakeUniversalAnalyticsEvent(
                    "test user 1",
                    "rate limit test category",
                    "rate limit test action");
            eventTracker.TrackEvent(analyticsEventForUser1);

            IUniversalAnalyticsEvent analyticsEventForUser2 = eventFactory.MakeUniversalAnalyticsEvent(
                "test user 2",
                "test category",
                "test action");
            eventTracker.TrackEvent(analyticsEventForUser2);
        }
    }
}
