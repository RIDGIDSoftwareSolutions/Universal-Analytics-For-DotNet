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
        private PostDataBuilder postDataBuilder;
        private IUniversalAnalyticsEvent analyticsEvent;
        private string measurementProtocolVersion = "1";
        private string trackingId = "UA-52123335-1";
        private string anonymousClientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";

        [SetUp]
        public void SetUp()
        {
            postDataBuilder = new PostDataBuilder();

            analyticsEvent = MockRepository.GenerateMock<IUniversalAnalyticsEvent>();
            analyticsEvent.Expect(mock => mock.MeasurementProtocolVersion)
                .Return(measurementProtocolVersion);
            analyticsEvent.Expect(mock => mock.TrackingId)
                .Return(trackingId);
            analyticsEvent.Expect(mock => mock.AnonymousClientId)
                .Return(anonymousClientId);
            analyticsEvent.Expect(mock => mock.EventAction)
                .Return(eventAction);
            analyticsEvent.Expect(mock => mock.EventCategory)
                .Return(eventCategory);
            analyticsEvent.Expect(mock => mock.EventLabel)
                .Return(eventLabel);
            analyticsEvent.Expect(mock => mock.EventValue)
                .Return(eventValue);
        }

        [Test]
        public void ItPutsTheMeasurementProtocolVersionInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_VERSION, measurementProtocolVersion);
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
        public void ItDoesntAddTheEventLabelIfItIsNull()
        {
            analyticsEvent.Expect(mock => mock.EventLabel)
                .Return(null)
                .Repeat.Any();
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
            analyticsEvent.Expect(mock => mock.EventValue)
                .Return(null)
                .Repeat.Any();
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_EVENT_VALUE]);
        }

        private void ValidateKeyValuePairIsSetOnPostData(string key, string expectedValue)
        {
            string postData = postDataBuilder.BuildPostDataString(analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            string actualValue = nameValueCollection[key];

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
