using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        protected string anonymousClientId = "anonymous client id";
        protected string eventCategory = "event category";
        protected string eventAction = "event action";
        protected string eventLabel = "event label";
        protected string eventValue = "500";

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
                AnonymousClientId = anonymousClientId,
                EventCategory = eventCategory,
                EventAction = eventAction,
                EventLabel = eventLabel,
                EventValue = eventValue
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
            analyticsEvent.EventAction = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheActionIsNull()
        {
            analyticsEvent.EventAction = null;

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Action", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsWhiteSpace()
        {
            analyticsEvent.EventCategory = "   ";

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheCategoryIsNull()
        {
            analyticsEvent.EventCategory = null;

            Exception exception = Assert.Throws<ArgumentException>(() => postDataBuilder.BuildPostDataString(analyticsEvent));
            Assert.AreEqual("analyticsEvent.Category", exception.Message);
        }

        [Test]
        public void ItPutsTheVersionInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_VERSION, version);
        }

        [Test]
        public void ItPutsTheTrackingIdInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_TRACKING_ID, trackingId);
        }

        [Test]
        public void ItPutsTheAnonymousClientIdInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_ANONYMOUS_CLIENT_ID, anonymousClientId);
        }

        [Test]
        public void ItSetsTheHitTypeToEventInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_HIT_TYPE, PostDataBuilder.HIT_TYPE_EVENT);
        }

        [Test]
        public void ItPutsTheEventActionInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_ACTION, eventAction);
        }

        [Test]
        public void ItPutsTheEventCategoryInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_CATEGORY, eventCategory);
        }

        [Test]
        public void ItPutsTheEventLabelInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_LABEL, eventLabel);
        }

        [Test]
        public void ItDoesntAddTheEventLavelIfItIsNull()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.EventLabel = null;
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_EVENT_LABEL]);
        }

        [Test]
        public void ItPutsTheEventValueInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_VALUE, eventValue);
        }

        [Test]
        public void ItDoesntAddTheEventValueIfItIsNull()
        {
            SetupMocksForConfigCalls(true, true);
            analyticsEvent.EventValue = null;
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_EVENT_VALUE]);
        }

        private void ValidateKeyValuePairIsSetOnPostData(string key, string expectedValue)
        {
            SetupMocksForConfigCalls(true, true);
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            string actualValue = nameValueCollection[key];

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
