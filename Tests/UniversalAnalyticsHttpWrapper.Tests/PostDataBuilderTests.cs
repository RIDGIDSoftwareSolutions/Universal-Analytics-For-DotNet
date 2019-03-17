﻿using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class PostDataBuilderTests
    {
        private PostDataBuilder _postDataBuilder;
        private IUniversalAnalyticsEvent _analyticsEvent;

        private string _trackingId = "UA-52123335-1";
        private string _anonymousClientId = "anonymous client id";
        private string _eventCategory = "event category";
        private string _eventAction = "event action";
        private string _eventLabel = "event label";
        private string _eventValue = "500";
        private string _userId = "user id";
        private bool _nonInteractionHit = true;

        [SetUp]
        public void SetUp()
        {
            _postDataBuilder = new PostDataBuilder();

            _analyticsEvent = MockRepository.GenerateMock<IUniversalAnalyticsEvent>();
            _analyticsEvent.Expect(mock => mock.TrackingId)
                .Return(_trackingId);
            _analyticsEvent.Expect(mock => mock.AnonymousClientId)
                .Return(_anonymousClientId);
            _analyticsEvent.Expect(mock => mock.EventAction)
                .Return(_eventAction);
            _analyticsEvent.Expect(mock => mock.EventCategory)
                .Return(_eventCategory);
            _analyticsEvent.Expect(mock => mock.EventLabel)
                .Return(_eventLabel);
            _analyticsEvent.Expect(mock => mock.EventValue)
                .Return(_eventValue);
            _analyticsEvent.Expect(m => m.UserId)
                .Return(_userId);
            _analyticsEvent.Expect(mock => mock.NonInteractionHit)
                .Return(_nonInteractionHit);
        }

        [Test]
        public void ItPutsTheMeasurementProtocolVersionInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_VERSION, EventTracker.MEASUREMENT_PROTOCOL_VERSION);
        }

        [Test]
        public void ItPutsTheTrackingIdInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_TRACKING_ID, _trackingId);
        }

        [Test]
        public void ItPutsTheAnonymousClientIdInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_ANONYMOUS_CLIENT_ID, _anonymousClientId);
        }

        [Test]
        public void ItPutsTheUserIdInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_USER_ID, this._userId);
        }

        [Test]
        public void ItPutsTheNonInteractionHitInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_NON_INTERACTION_HIT, "1");
        }

        [Test]
        public void ItDoesntAddTheNonInterationHitIfItIsFalse()
        {
            _analyticsEvent.Expect(mock => mock.NonInteractionHit)
                .Return(false)
                .Repeat.Any();
            string postData = _postDataBuilder.BuildPostDataString(string.Empty, _analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_NON_INTERACTION_HIT]);
        }

        [Test]
        public void ItSetsTheHitTypeToEventInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_HIT_TYPE, PostDataBuilder.HIT_TYPE_EVENT);
        }

        [Test]
        public void ItPutsTheEventActionInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_ACTION, _eventAction);
        }

        [Test]
        public void ItPutsTheEventCategoryInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_CATEGORY, _eventCategory);
        }

        [Test]
        public void ItPutsTheEventLabelInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_LABEL, _eventLabel);
        }

        [Test]
        public void ItDoesntAddTheEventLabelIfItIsNull()
        {
            _analyticsEvent.Expect(mock => mock.EventLabel)
                .Return(null)
                .Repeat.Any();
            string postData = _postDataBuilder.BuildPostDataString(string.Empty, _analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_EVENT_LABEL]);
        }

        [Test]
        public void ItPutsTheEventValueInTheString()
        {
            ValidateKeyValuePairIsSetOnPostData(PostDataBuilder.PARAMETER_KEY_EVENT_VALUE, _eventValue);
        }

        [Test]
        public void ItDoesntAddTheEventValueIfItIsNull()
        {
            _analyticsEvent.Expect(mock => mock.EventValue)
                .Return(null)
                .Repeat.Any();
            string postData = _postDataBuilder.BuildPostDataString(string.Empty, _analyticsEvent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postData);

            Assert.Null(nameValueCollection[PostDataBuilder.PARAMETER_KEY_EVENT_VALUE]);
        }


        //added by Dmitry Klymenko, 15 Mar 2019
        [Test]
        public void ItPutsAdditionalPayload()
        {
            NameValueCollection nvc = new NameValueCollection(1);
            nvc.Add("qt", "560");

            ValidateKeyValuePairIsSetOnPostData("qt", "560", nvc);
        }

        private void ValidateKeyValuePairIsSetOnPostData(string key, string expectedValue, NameValueCollection customPayload = null)
        {
            string postDataString = _postDataBuilder.BuildPostDataString(EventTracker.MEASUREMENT_PROTOCOL_VERSION, _analyticsEvent, customPayload);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(postDataString);
            string actualValue = nameValueCollection[key];
            Assert.AreEqual(expectedValue, actualValue);

            var postDataCollection = _postDataBuilder.BuildPostDataCollection(EventTracker.MEASUREMENT_PROTOCOL_VERSION, _analyticsEvent, customPayload);
            string actualCollectionValue = postDataCollection.Single(s => s.Key == key).Value;
            Assert.AreEqual(expectedValue, actualCollectionValue);
        }
    }
}
