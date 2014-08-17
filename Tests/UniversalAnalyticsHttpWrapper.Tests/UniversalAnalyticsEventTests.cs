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
    public class UniversalAnalyticsEventTests
    {
        private IConfigurationManager configurationManagerMock;
        private IAppSettings appSettingsMock;

        //TODO change all private to private
        private string measurementProtocolVersion = "1";
        private string trackingId = "A-XXXXXX-YY";
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
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, "  ", eventCategory, eventAction));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, null, eventCategory, eventAction));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItSetsTheAnonymousClientIdInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, eventCategory, eventAction);

            Assert.AreEqual(anonymousClientId, universalAnalyticsEvent.AnonymousClientId);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, "  ", eventAction));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, null, eventAction));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItSetsTheEventCategoryInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, eventCategory, eventAction);

            Assert.AreEqual(eventCategory, universalAnalyticsEvent.EventCategory);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, eventCategory, "  "));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, eventCategory, null));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItSetsEventActionInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);

            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(configurationManagerMock, anonymousClientId, eventCategory, eventAction);

            Assert.AreEqual(eventAction, universalAnalyticsEvent.EventAction);
        }

        [Test]
        public void ItSetsEventLabelInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);

            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                configurationManagerMock, anonymousClientId, eventCategory, eventAction, eventLabel);

            Assert.AreEqual(eventLabel, universalAnalyticsEvent.EventLabel);
        }

        [Test]
        public void ItSetsEventValueInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);

            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                configurationManagerMock, 
                anonymousClientId, 
                eventCategory, 
                eventAction, 
                eventLabel, 
                eventValue);

            Assert.AreEqual(eventValue, universalAnalyticsEvent.EventValue);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheVersionIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(false, true);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(UniversalAnalyticsEvent.APP_KEY_UNIVERSAL_ANALYTICS_VERSION);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => new UniversalAnalyticsEvent(
                configurationManagerMock, 
                anonymousClientId, 
                eventCategory, 
                eventAction, 
                eventLabel, 
                eventValue));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItSetsTheMeasurementProtocolVersionInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);

            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                configurationManagerMock, anonymousClientId, eventCategory, eventAction, eventLabel, eventValue);

            Assert.AreEqual(measurementProtocolVersion, universalAnalyticsEvent.MeasurementProtocolVersion);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheTrackingIdIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(true, false);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(UniversalAnalyticsEvent.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => new UniversalAnalyticsEvent(
              configurationManagerMock, 
              anonymousClientId,
              eventCategory,
              eventAction,
              eventLabel,
              eventValue));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItSetsTheTrackingIdInTheConstructor()
        {
            SetupMocksForConfigCalls(true, true);

            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                configurationManagerMock, anonymousClientId, eventCategory, eventAction, eventLabel, eventValue);

            Assert.AreEqual(trackingId, universalAnalyticsEvent.TrackingId);
        }

        private void SetupMocksForConfigCalls(bool setupVersionLookup, bool setupTrackingIdLookup)
        {
            if (setupVersionLookup)
            {
                appSettingsMock.Expect(mock => mock[UniversalAnalyticsEvent.APP_KEY_UNIVERSAL_ANALYTICS_VERSION])
                    .Return(measurementProtocolVersion);
            }

            if (setupTrackingIdLookup)
            {
                appSettingsMock.Expect(mock => mock[UniversalAnalyticsEvent.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                    .Return(trackingId);
            }
        }
    }
}
