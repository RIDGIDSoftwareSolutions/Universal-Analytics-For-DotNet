using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test, Ignore("Integration test.")]
        public void SampleCodeForGitHubReadMe()
        {
            //create a new EvenTracker
            IEventTracker eventTracker = new EventTracker();
            //create a new event to pass to the event tracker
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
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

        [Test, Ignore("Integration test.")]
        public void Send100EventsAsQuicklyAsPossibleAndARateLimitStillWontOccur()
        {
            EventTracker eventTracker = new EventTracker();

            for (int i = 1; i <= 100; i++)
            {
                try
                {
                    UniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
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

        [Test, Ignore("Integration test.")]
        public void SendingEventsFromTwoDifferentUsersShowsUpAsTwoActiveSessionsInUniversalAnalytics()
        {
            //you actually have to watch the analytics property to see this one happening. No assertion necessary.
            EventTracker eventTracker = new EventTracker();

            UniversalAnalyticsEvent analyticsEventForUser1 = new UniversalAnalyticsEvent(
                    "test user 1",
                    "rate limit test category",
                    "rate limit test action");
            eventTracker.TrackEvent(analyticsEventForUser1);

            UniversalAnalyticsEvent analyticsEventForUser2 = new UniversalAnalyticsEvent(
                "test user 2",
                "test category",
                "test action");
            eventTracker.TrackEvent(analyticsEventForUser2);
        }
    }
}
