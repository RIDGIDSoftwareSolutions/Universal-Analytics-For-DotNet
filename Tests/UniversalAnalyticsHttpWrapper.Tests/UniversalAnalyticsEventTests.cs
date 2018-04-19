using NUnit.Framework;
using System;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventTests
    {
        private string _trackingId = "A-XXXXXX-YY";
        private string _clientId = "anonymous client id";
        private string _eventCategory = "event category";
        private string _eventAction = "event action";
        private string _eventLabel = "event label";
        private string _eventValue = "500";
        private string _userId = "user id";

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheTrackingIdIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                "   ",
                _clientId,
                _eventCategory,
                _eventAction,
                _eventLabel));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.TrackingId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheTrackingIdAndUserIdAreNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                null,
                _clientId,
                _eventCategory,
                _eventAction,
                _eventLabel,
                null));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.TrackingId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdAndUserIdAreWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId, 
                "  ", 
                _eventCategory,
                _eventAction,
                _eventLabel,
                "   "));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.AnonymousClientId || analyticsEvent.UserId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheUserIdIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId,
                null,
                _eventCategory,
                _eventAction,
                _eventLabel,
                null));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.AnonymousClientId || analyticsEvent.UserId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        [Test]
        public void ItSetsTheTrackingIdInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = GetFullyPopulatedEventUsingConstructor();

            Assert.AreEqual(_trackingId, universalAnalyticsEvent.TrackingId);
        }

        [Test]
        public void ItSetsTheAnonymousClientIdInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = GetFullyPopulatedEventUsingConstructor();

            Assert.AreEqual(this._clientId, universalAnalyticsEvent.AnonymousClientId);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId, 
                "  ",
                _eventAction,
                _eventLabel));
            
            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE, 
                "analyticsEvent.EventCategory");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId, 
                null,
                _eventAction,
                _eventLabel));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventCategory");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItSetsTheEventCategoryInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId, 
                _eventCategory,
                _eventAction,
                _eventLabel);

            Assert.AreEqual(_eventCategory, universalAnalyticsEvent.EventCategory);
        }

        [Test]
        public void ItSetsTheUserIdInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId,
                _clientId,
                _eventCategory,
                _eventAction,
                _eventLabel,
                _eventValue,
                _userId);

            Assert.AreEqual(this._userId, universalAnalyticsEvent.UserId);
        }


        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId,
                _eventCategory,
                "  ",
                _eventLabel));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventAction");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId, 
                _eventCategory,
                null,
                _eventLabel));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventAction");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItSetsEventActionInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId, 
                _clientId, 
                _eventCategory,
                _eventAction,
                _eventLabel);

            Assert.AreEqual(_eventAction, universalAnalyticsEvent.EventAction);
        }

        [Test]
        public void ItSetsEventLabelInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId, 
                this._clientId, 
                _eventCategory, 
                _eventAction, 
                _eventLabel);

            Assert.AreEqual(_eventLabel, universalAnalyticsEvent.EventLabel);
        }

        [Test]
        public void ItSetsEventValueInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId,
                this._clientId, 
                _eventCategory, 
                _eventAction, 
                _eventLabel, 
                _eventValue);

            Assert.AreEqual(_eventValue, universalAnalyticsEvent.EventValue);
        }

        private UniversalAnalyticsEvent GetFullyPopulatedEventUsingConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                _trackingId,
                this._clientId,
                _eventCategory,
                _eventAction,
                _eventLabel,
                _eventValue);
            return universalAnalyticsEvent;
        }
    }
}
