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
            SetupMocksForConfigCalls(true, true);
            IUniversalAnalyticsEvent analyticsEvent = factory.MakeUniversalAnalyticsEvent(
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(measurementProtocolVersion, analyticsEvent.MeasurementProtocolVersion);
            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(anonymousClientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheVersionIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(false, true);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_VERSION);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => factory.MakeUniversalAnalyticsEvent(
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheTrackingIdIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(true, false);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => factory.MakeUniversalAnalyticsEvent(
              anonymousClientId,
              eventCategory,
              eventAction,
              eventLabel,
              eventValue));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        private void SetupMocksForConfigCalls(bool setupVersionLookup, bool setupTrackingIdLookup)
        {
            if (setupVersionLookup)
            {
                appSettingsMock.Expect(mock => mock[UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_VERSION])
                    .Return(measurementProtocolVersion);
            }

            if (setupTrackingIdLookup)
            {
                appSettingsMock.Expect(mock => mock[UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                    .Return(trackingId);
            }
        }
    }
}
