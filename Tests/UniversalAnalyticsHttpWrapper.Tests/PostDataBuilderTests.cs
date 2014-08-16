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
    public class PostDataBuilderTests
    {
        protected IConfigurationManager configurationManagerMock;
        protected IAppSettings appSettingsMock;
        protected PostDataBuilder postDataBuilder;
        protected UniversalAnalyticsEvent analyticsEvent;
        protected string version = "1";
        protected string trackingId = "UA-52123335-1";

        [SetUp]
        public void SetUp()
        {
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();
            appSettingsMock = MockRepository.GenerateMock<IAppSettings>();

            postDataBuilder = new PostDataBuilder(configurationManagerMock);

            configurationManagerMock.Expect(mock => mock.AppSettings)
                .Return(appSettingsMock);

            //an event that has enough data populated that it could be logged successfully
            analyticsEvent = new UniversalAnalyticsEvent(postDataBuilder)
            {
                AnonymousClientId = "client id",
                Category = "category",
                Action = "action",
                Label = "label"
            };
        }

        private void SetupMocksForConfigCalls(bool setupVersionLookup, bool setupTrackingIdLookup)
        {
            if (setupVersionLookup)
            {
                appSettingsMock.Expect(mock => mock[PostDataBuilder.APP_KEY_UNIVERSAL_ANALYTICS_VERSION])
                    .Return(version);
            }

            if (setupTrackingIdLookup)
            {
                appSettingsMock.Expect(mock => mock[PostDataBuilder.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID])
                    .Return(trackingId);
            }
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheVersionIsntSetInTheConfig()
        {
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(PostDataBuilder.APP_KEY_UNIVERSAL_ANALYTICS_VERSION);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItThrowsAConfigEntryMissingExceptionIfTheTrackingIdIsntSetInTheConfig()
        {
            SetupMocksForConfigCalls(true, false);
            ConfigEntryMissingException expectedException = new ConfigEntryMissingException(PostDataBuilder.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);

            ConfigEntryMissingException exception = Assert.Throws<ConfigEntryMissingException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));

            Assert.AreEqual(expectedException.Message, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsWhiteSpace()
        {
            analyticsEvent.AnonymousClientId = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsNull()
        {
            analyticsEvent.AnonymousClientId = null;

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.AnonymousClientId", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheActionIsWhiteSpace()
        {
            analyticsEvent.Action = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheActionIsNull()
        {
            analyticsEvent.Action = null;

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsWhiteSpace()
        {
            analyticsEvent.Category = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsNull()
        {
            analyticsEvent.Category = null;

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

    }
}
