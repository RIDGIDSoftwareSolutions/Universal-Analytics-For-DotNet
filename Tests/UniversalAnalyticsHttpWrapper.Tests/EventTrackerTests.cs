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
        private EventTracker _eventTracker;
        private IUniversalAnalyticsEvent _analyticsEvent;
        private IPostDataBuilder _postDataBuilderMock;
        private IGoogleDataSender _googleDataSenderMock;

        [SetUp]
        public void SetUp()
        {
            _postDataBuilderMock = MockRepository.GenerateMock<IPostDataBuilder>();
            _googleDataSenderMock = MockRepository.GenerateMock<IGoogleDataSender>();
            _eventTracker = new EventTracker(_postDataBuilderMock, _googleDataSenderMock);

            _analyticsEvent = MockRepository.GenerateMock<IUniversalAnalyticsEvent>();
        }

        [Test]
        public void ItSendsTheDataToGoogle()
        {
            string expectedPostData = "some amazing string that matches what google requires";
            
            _postDataBuilderMock.Expect(mock => mock.BuildPostDataString(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            _eventTracker.TrackEvent(_analyticsEvent);

            _googleDataSenderMock.AssertWasCalled(mock => 
                mock.SendData(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public async Task ItSendsTheDataToGoogleAsync()
        {
            var expectedPostData = new List<KeyValuePair<string, string>>();

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            _googleDataSenderMock.Expect(mock => mock.SendDataAsync(
                    Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Return(Task.FromResult(0));
            
            await _eventTracker.TrackEventAsync(_analyticsEvent);

            _googleDataSenderMock.AssertWasCalled(mock =>
                mock.SendDataAsync(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public void ItBubblesExceptionsToTheTrackingResult()
        {
            var expectedException = new HttpException(400, "bad request");

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(new List<KeyValuePair<string, string>>());

            _googleDataSenderMock.Expect(mock => mock.SendData(Arg<Uri>.Is.Anything,
                    Arg<string>.Is.Anything))
                .Throw(expectedException);

            var result = _eventTracker.TrackEvent(_analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }

        [Test]
        public async void ItBubblesExceptionsToTheTrackingResultAsync()
        {
            var expectedException = new HttpException(400, "bad request");

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(new List<KeyValuePair<string, string>>());

            _googleDataSenderMock.Expect(mock => mock.SendDataAsync(Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Throw(expectedException);

            var result = await _eventTracker.TrackEventAsync(_analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }
    }
}
