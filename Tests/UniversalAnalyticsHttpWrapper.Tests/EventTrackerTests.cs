using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class EventTrackerTests
    {
        private EventTracker eventTracker;
        private IUniversalAnalyticsEvent analyticsEvent;
        private IPostDataBuilder postDataBuilderMock;
        private IGoogleDataSender googleDataSenderMock;

        [SetUp]
        public void SetUp()
        {
            postDataBuilderMock = MockRepository.GenerateMock<IPostDataBuilder>();
            googleDataSenderMock = MockRepository.GenerateMock<IGoogleDataSender>();
            eventTracker = new EventTracker(postDataBuilderMock, googleDataSenderMock);

            analyticsEvent = MockRepository.GenerateMock<IUniversalAnalyticsEvent>();
        }

        [Test]
        public void ItSendsTheDataToGoogle()
        {
            string expectedPostData = "some amazing string that matches what google requires";
            
            postDataBuilderMock.Expect(mock => mock.BuildPostDataString(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            eventTracker.TrackEvent(analyticsEvent);

            googleDataSenderMock.AssertWasCalled(mock => 
                mock.SendData(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public async Task ItSendsTheDataToGoogleAsync()
        {
            var expectedPostData = new List<KeyValuePair<string, string>>();

            postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            googleDataSenderMock.Expect(mock => mock.SendDataAsync(
                    Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Return(Task.FromResult(0));
            
            await eventTracker.TrackEventAsync(analyticsEvent);

            googleDataSenderMock.AssertWasCalled(mock =>
                mock.SendDataAsync(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public void ItBubblesExceptionsToTheTrackingResult()
        {
            var expectedException = new HttpException(400, "bad request");

            postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(new List<KeyValuePair<string, string>>());

            googleDataSenderMock.Expect(mock => mock.SendData(Arg<Uri>.Is.Anything,
                    Arg<string>.Is.Anything))
                .Throw(expectedException);

            var result = eventTracker.TrackEvent(analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }

        [Test]
        public async void ItBubblesExceptionsToTheTrackingResultAsync()
        {
            var expectedException = new HttpException(400, "bad request");

            postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(new List<KeyValuePair<string, string>>());

            googleDataSenderMock.Expect(mock => mock.SendDataAsync(Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Throw(expectedException);

            var result = await eventTracker.TrackEventAsync(analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }
    }
}
