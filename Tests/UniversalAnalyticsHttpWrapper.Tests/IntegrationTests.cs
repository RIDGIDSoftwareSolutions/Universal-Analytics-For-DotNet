using NUnit.Framework;
using System;
using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture, Ignore("Integration Tests")]
    public class IntegrationTests
    {
        private readonly EventTracker _eventTracker = new EventTracker();
        private readonly UniversalAnalyticsEventFactory _eventFactory = new UniversalAnalyticsEventFactory();

        [Test]
        public void UserIdEventIsTrackedCorrectly()
        {
            var userId = new UserId("user id");
            var userIdEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                userId,
                "userIdTestCategory",
                "userIdTestAction",
                "userIdTestLabel");

            var result = _eventTracker.TrackEvent(userIdEvent);

            Assert.IsFalse(result.Failed);
        }

        [Test]
        public void RandomClientIdIsTrackedCorrectly()
        {
            var clientId = new ClientId();
            var clientIdEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                clientId,
                "randomClientIdCat",
                "randomClientIdAction",
                "randomClientIdLabel");

            var result = _eventTracker.TrackEvent(clientIdEvent);

            Assert.IsFalse(result.Failed);
        }

        [Test]
        public void SuppliedGuidClientIdIsTrackedCorrectly()
        {
            var clientIdGuid = new Guid("86e54dd9-d3d2-4496-8731-218b3a87ffa2");
            var clientId = new ClientId(clientIdGuid);
            var clientIdEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                clientId,
                "guidClientIdCat",
                "guidClientIdAction",
                "guidClientIdLabel");

            var result = _eventTracker.TrackEvent(clientIdEvent);

            Assert.IsFalse(result.Failed);
        }

        [Test]
        public void SeedStringClientIdIsTrackedCorrectly()
        {
            var clientId = new ClientId("seed string");
            var clientIdEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                clientId,
                "seedClientIdCat",
                "seedClientIdAction",
                "seedClientIdLabel");

            var result = _eventTracker.TrackEvent(clientIdEvent);

            Assert.IsFalse(result.Failed);
        }

        [Test]
        public void Send100EventsAsQuicklyAsPossibleAndARateLimitStillWontOccur()
        {
            var randomCid = new ClientId("rate limit test cid");
            for (var i = 1; i <= 100; i++)
            {
                var analyticsEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                    randomCid,
                    "rate limit test category",
                    "rate limit test action",
                    "rate limit test label",
                    i.ToString());
                var result = _eventTracker.TrackEvent(analyticsEvent);

                if (result.Failed)
                {
                    Assert.Fail("Failed after " + i + " transmissions with the following exception: " + result.Exception.Message);
                }
            }
        }

        [Test]
        public void SendingEventsFromTwoDifferentUsersShowsUpAsTwoActiveSessionsInUniversalAnalytics()
        {
            //you actually have to watch the analytics property to see this one happening.
            var client1 = new ClientId();
            var client2 = new ClientId();

            var analyticsEventForClient1 = _eventFactory.MakeUniversalAnalyticsEvent(
                client1,
                "rate limit test category",
                "rate limit test action",
                "rate limit test label");
            var result1 = _eventTracker.TrackEvent(analyticsEventForClient1);

            var analyticsEventForClient2 = _eventFactory.MakeUniversalAnalyticsEvent(
                client2,
                "rate limit test category",
                "rate limit test action",
                "rate limit test label");
            var result2 = _eventTracker.TrackEvent(analyticsEventForClient2);

            // Assert the tracking didn't result in a failure.
            Assert.IsFalse(result1.Failed);
            Assert.IsFalse(result2.Failed);
        }

        [Test]
        public void NonInteractionEventsDontStartNewSessions()
        {
            var userId = new UserId("user id without a new session");
            var userIdEvent = _eventFactory.MakeUniversalAnalyticsEvent(
                userId,
                "nonInteractdionHitCategory",
                "nonInteractdionHitAction",
                "nonInteractdionHitLabel",
                nonInteractionEvent: true);

            var result = _eventTracker.TrackEvent(userIdEvent);

            Assert.IsFalse(result.Failed);
        }

        [Test]
        public void SampleCodeForGitHubReadMeUsingFactoryToGetEventObject()
        {
            // Use your favorite dependency injection framework for the tracker and factory. Singletons preferred.
            IEventTracker eventTracker = new EventTracker();
            // The factory pulls your tracking ID from the .config so you don't have to.
            IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();

            var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
                // Required if no user id. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
                "35009a79-1a05-49d7-b876-2b884d0f825b",
                // Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                // Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                // Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                // Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10",
                // Required if no client id. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
                "user-id"
            );

            // Exceptions are contained in the result object and not thrown for stability reasons.
            var trackingResult = eventTracker.TrackEvent(analyticsEvent);

            Assert.IsFalse(trackingResult.Failed);
        }

        [Test]
        public void SampleCodeForGitHubReadMeManuallyConstructingEventObject()
        {
            //Create a new EventTracker
            IEventTracker eventTracker = new EventTracker();
            //Create a new event to pass to the event tracker
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
                //Required. The universal analytics tracking id for the property 
                // that events will be logged to. If you don't want to pass this every time, set the UniversalAnalytics.TrackingId 
                // app setting and use the UniversalAnalyticsEventFactory to get instances of this class.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#tid for details.
                "UA-52982625-3",
                //Required. client id. 
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
                "10",
                //Required if no client id. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
                "user-id");
            var result = eventTracker.TrackEvent(analyticsEvent);

            Assert.IsFalse(result.Failed);
        }

        //added by Dmitrry Klymenko, 15 Mar 2019 
        [Test]
        public void SampleCodeForGitHubReadMeAddingcustomPayload()
        {
            IEventTracker eventTracker = new EventTracker();

            //This is a simple pass-thru of other (not yet supported) Measurement Protocol fields which are not directly related to the Event tracking but might come handy
            // Using custom payload you can send to GA event contextual information such as page or screen where event happened, Custom Dimensions, Custom Metrics
            // GA Measurement Protocol parameters reference: https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters
            //Please see below some commonly used parameters

            //Document Location - full uri to the page url
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#dl
            eventTracker.AddToCustomPayload("dl", "https://mytesturl.com/");

            //Document title - page title, if you want GA to know page/screen title where event has happened
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#dt
            eventTracker.AddToCustomPayload("dt", "My test title");

            //Screen Name - name of the screen where event has happened
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cd
            eventTracker.AddToCustomPayload("cd", "My screen name");

            //Data source. You can use your app name here
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ds
            eventTracker.AddToCustomPayload("ds", "App Name");

            //Custom Dimension. You need to create it first in your GA admin interface
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cd_
            eventTracker.AddToCustomPayload("cd1", "Custom Dimension 1 value");

            //Custom Metric. You need to create it first in your GA admin interface
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cm_
            eventTracker.AddToCustomPayload("cm1", "100");

            //Queue Time. We can modify event time by suppliyng queue time for the hit up to 4 hours in milliseconds
            //https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#qt
            eventTracker.AddToCustomPayload("qt", "560");


            IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();

            var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
                // Required if no user id. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
                "35009a79-1a05-49d7-b876-2b884d0f825b",
                // Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                // Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                // Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                // Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10",
                // Required if no client id. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
                "user-id"
            );

            var result = eventTracker.TrackEvent(analyticsEvent);

            Assert.IsFalse(result.Failed);
        }

    }
}