using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventFactoryTests
    {
        private IConfigurationManager configurationManagerMock;
        private IAppSettings appSettingsMock;
        UniversalAnalyticsEventFactory factory;
        private string measurementProtocolVersion = "measurementProtocolVersion";
        private string trackingId = "tracking id";
        private string anonymousClientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";

        [SetUp]
        public void SetUp()
        {
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();
            appSettingsMock = MockRepository.GenerateMock<IAppSettings>();

            configurationManagerMock.Expect(mock => mock.AppSettings)
                .Return(appSettingsMock);

             factory = new UniversalAnalyticsEventFactory(configurationManagerMock);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEvent()
        {
            SetupMockForGettingTrackingIdFromConfig(true);
            IUniversalAnalyticsEvent analyticsEvent = factory.MakeUniversalAnalyticsEvent(
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(anonymousClientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheTrackingIdIsntSetInTheConfig()
        {
            SetupMockForGettingTrackingIdFromConfig(false);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => factory.MakeUniversalAnalyticsEvent(
              anonymousClientId,
              eventCategory,
              eventAction,
              eventLabel,
              eventValue));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        private void SetupMockForGettingTrackingIdFromConfig(bool setupTrackingIdLookup)
        {
            if (setupTrackingIdLookup)
            {
                appSettingsMock.Expect(mock => mock[UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                    .Return(trackingId);
            }
        }
    }
}
