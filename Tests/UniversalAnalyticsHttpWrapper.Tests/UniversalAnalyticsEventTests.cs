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
        private string trackingId = "A-XXXXXX-YY";
        private string anonymousClientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheTrackingIdIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                "   ",
                anonymousClientId,
                eventCategory,
                eventAction));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.TrackingId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheTrackingIdIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                null,
                anonymousClientId,
                eventCategory,
                eventAction));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.TrackingId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                "  ", 
                eventCategory, 
                eventAction));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.AnonymousClientId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheAnonymousClientIdIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                null, 
                eventCategory, 
                eventAction));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.AnonymousClientId");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItSetsTheTrackingIdInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = GetFullyPopulatedEventUsingConstructor();

            Assert.AreEqual(trackingId, universalAnalyticsEvent.TrackingId);
        }

        [Test]
        public void ItSetsTheAnonymousClientIdInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = GetFullyPopulatedEventUsingConstructor();

            Assert.AreEqual(anonymousClientId, universalAnalyticsEvent.AnonymousClientId);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                "  ", 
                eventAction));
            
            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE, 
                "analyticsEvent.EventCategory");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventCategoryIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                null, 
                eventAction));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventCategory");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItSetsTheEventCategoryInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                eventCategory, 
                eventAction);

            Assert.AreEqual(eventCategory, universalAnalyticsEvent.EventCategory);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsWhiteSpace()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId,
                eventCategory, 
                "  "));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventAction");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItThrowsAnArgumentExceptionIfTheEventActionIsNull()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                eventCategory, 
                null));

            string expectedMessage = string.Format(
                UniversalAnalyticsEvent.EXCEPTION_MESSAGE_PARAMETER_CANNOT_BE_NULL_OR_WHITESPACE,
                "analyticsEvent.EventAction");
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void ItSetsEventActionInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                eventCategory, 
                eventAction);

            Assert.AreEqual(eventAction, universalAnalyticsEvent.EventAction);
        }

        [Test]
        public void ItSetsEventLabelInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                trackingId, 
                anonymousClientId, 
                eventCategory, 
                eventAction, 
                eventLabel);

            Assert.AreEqual(eventLabel, universalAnalyticsEvent.EventLabel);
        }

        [Test]
        public void ItSetsEventValueInTheConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                trackingId,
                anonymousClientId, 
                eventCategory, 
                eventAction, 
                eventLabel, 
                eventValue);

            Assert.AreEqual(eventValue, universalAnalyticsEvent.EventValue);
        }

        private UniversalAnalyticsEvent GetFullyPopulatedEventUsingConstructor()
        {
            UniversalAnalyticsEvent universalAnalyticsEvent = new UniversalAnalyticsEvent(
                trackingId,
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);
            return universalAnalyticsEvent;
        }
    }
}
