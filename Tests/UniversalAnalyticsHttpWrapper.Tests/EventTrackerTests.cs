using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalAnalyticsHttpWrapper;
using UniversalAnalyticsHttpWrapper.Exceptions;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class EventTrackerTests
    {
        protected EventTracker eventTracker;
        protected UniversalAnalyticsEvent analyticsEvent;
        protected IPostDataBuilder postDataBuilderMock;
        protected IConfigurationManager configurationManagerMock;
        protected IAppSettings appSettingsMock;
        protected IGoogleDataSender googleDataSenderMock;
        protected string version = "1";
        protected string trackingId = "UA-52123335-1";

        [SetUp]
        public void SetUp()
        {
            postDataBuilderMock = MockRepository.GenerateMock<IPostDataBuilder>();
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();
            appSettingsMock = MockRepository.GenerateMock<IAppSettings>();
            googleDataSenderMock = MockRepository.GenerateMock<IGoogleDataSender>();
            eventTracker = new EventTracker(configurationManagerMock, postDataBuilderMock, googleDataSenderMock);

            //an event that has enough data populated that it could be logged successfully
            analyticsEvent = new UniversalAnalyticsEvent(postDataBuilderMock)
            {
                AnonymousClientId = "client id",
                Category = "category",
                Action = "action",
                Label = "label"
            };

            configurationManagerMock.Expect(mock => mock.AppSettings)
                .Return(appSettingsMock);
        }

        private void SetupMocksForConfigCalls(bool setupVersionLookup, bool setupTrackingIdLookup)
        {
            if (setupVersionLookup)
            {
                appSettingsMock.Expect(mock => mock[EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_VERSION])
                    .Return(version);
            }

            if(setupTrackingIdLookup)
            {
                appSettingsMock.Expect(mock => mock[EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                    .Return(trackingId);
            }
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsWhiteSpace()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.AnonymousClientId = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsNull()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.AnonymousClientId = null;

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheActionIsWhiteSpace()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.Action = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheActionIsNull()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.Action = null;

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsWhiteSpace()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.Category = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsNull()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.Category = null;

            Exception exception = Assert.Throws<ArgumentException>(() => eventTracker.TrackEvent(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItGetsTheVersionAndTrackingIdFromTheConfig()
        {
            SetupMocksForConfigCalls(true, true);
            eventTracker.TrackEvent(analyticsEvent);

            appSettingsMock.AssertWasCalled(mock => mock[EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_VERSION]);
            appSettingsMock.AssertWasCalled(mock => mock[EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID]);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheVersionIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(false, true);

            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_VERSION);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => eventTracker.TrackEvent(analyticsEvent));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheTrackingIdIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(true, false);

            appSettingsMock.Expect(mock => mock[EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                .Return(null);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(EventTracker.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => eventTracker.TrackEvent(analyticsEvent));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItConstructsAStringPayloadToPost()
        {
            SetupMocksForConfigCalls(true, true);

            eventTracker.TrackEvent(analyticsEvent);

            postDataBuilderMock.AssertWasCalled(mock => mock.BuildPostDataString(version, trackingId, analyticsEvent));
        }

        [Test]
        public void ItSendsTheDataToGoogle()
        {
            SetupMocksForConfigCalls(true, true);
            string expectedPostData = "some amazing string that matches what google requires";
            
            postDataBuilderMock.Expect(mock => mock.BuildPostDataString(
                Arg<string>.Is.Anything,
                Arg<string>.Is.Anything,
                Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            eventTracker.TrackEvent(analyticsEvent);

            googleDataSenderMock.AssertWasCalled(mock => mock.SendData(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }
    }
}
