using System;
using NUnit.Framework;
using Rhino.Mocks;
using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventFactoryTests
    {
        private IConfigurationManager configurationManagerMock;
        UniversalAnalyticsEventFactory factory;
        private string trackingId = "tracking id";
        private string clientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";
        private string userId = "user id";
        private Guid cidGuid = Guid.Empty;
        private ClientId clientIdFromGuid;
        private ClientId clientIdFromString;
        private UserId userIdFromString;

        [SetUp]
        public void SetUp()
        {
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();
            factory = new UniversalAnalyticsEventFactory(configurationManagerMock);
            clientIdFromGuid = new ClientId(cidGuid);
            clientIdFromString = new ClientId(clientId);
            userIdFromString = new UserId(userId);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEvent()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(
                clientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue,
                userId);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(clientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
            Assert.AreEqual(userId, analyticsEvent.UserId);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithGuidClientId()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(new ClientId(cidGuid),
                eventCategory, eventAction, eventLabel, eventValue);

            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(clientIdFromGuid.Id.ToString(), analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithStringClientId()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(new ClientId(clientId),
                eventCategory, eventAction, eventLabel, eventValue);

            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(clientIdFromString.Id.ToString(), analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithUserId()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(new UserId(userId), 
                eventCategory, eventAction, eventLabel, eventValue);

            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
            Assert.AreEqual(userIdFromString.Id, analyticsEvent.UserId);
        }
    }
}
