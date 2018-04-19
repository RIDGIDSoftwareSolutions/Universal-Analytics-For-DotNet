using System;
using NUnit.Framework;
using Rhino.Mocks;
using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventFactoryTests
    {
        private IConfigurationManager _configurationManagerMock;
        UniversalAnalyticsEventFactory _factory;
        private string _trackingId = "tracking id";
        private string _clientId = "anonymous client id";
        private string _eventCategory = "event category";
        private string _eventAction = "event action";
        private string _eventLabel = "event label";
        private string _eventValue = "500";
        private string _userId = "user id";
        private Guid _cidGuid = Guid.Empty;
        private ClientId _clientIdFromGuid;
        private ClientId _clientIdFromString;
        private UserId _userIdFromString;

        [SetUp]
        public void SetUp()
        {
            _configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();
            _factory = new UniversalAnalyticsEventFactory(_configurationManagerMock);
            _clientIdFromGuid = new ClientId(_cidGuid);
            _clientIdFromString = new ClientId(_clientId);
            _userIdFromString = new UserId(_userId);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEvent()
        {
            _configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(_trackingId);

            var analyticsEvent = _factory.MakeUniversalAnalyticsEvent(
                _clientId,
                _eventCategory,
                _eventAction,
                _eventLabel,
                _eventValue,
                _userId);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(_trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(_clientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(_eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(_eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(_eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(_eventValue, analyticsEvent.EventValue);
            Assert.AreEqual(_userId, analyticsEvent.UserId);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithGuidClientId()
        {
            _configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(_trackingId);

            var analyticsEvent = _factory.MakeUniversalAnalyticsEvent(new ClientId(_cidGuid),
                _eventCategory, _eventAction, _eventLabel, _eventValue);

            Assert.AreEqual(_trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(_clientIdFromGuid.Id.ToString(), analyticsEvent.AnonymousClientId);
            Assert.AreEqual(_eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(_eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(_eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(_eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithStringClientId()
        {
            _configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(_trackingId);

            var analyticsEvent = _factory.MakeUniversalAnalyticsEvent(new ClientId(_clientId),
                _eventCategory, _eventAction, _eventLabel, _eventValue);

            Assert.AreEqual(_trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(_clientIdFromString.Id.ToString(), analyticsEvent.AnonymousClientId);
            Assert.AreEqual(_eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(_eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(_eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(_eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEventWithUserId()
        {
            _configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(_trackingId);

            var analyticsEvent = _factory.MakeUniversalAnalyticsEvent(new UserId(_userId), 
                _eventCategory, _eventAction, _eventLabel, _eventValue);

            Assert.AreEqual(_trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(_eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(_eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(_eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(_eventValue, analyticsEvent.EventValue);
            Assert.AreEqual(_userIdFromString.Id, analyticsEvent.UserId);
        }
    }
}
