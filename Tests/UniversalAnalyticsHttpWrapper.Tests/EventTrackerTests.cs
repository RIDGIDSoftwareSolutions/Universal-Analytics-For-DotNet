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
        protected IGoogleDataSender googleDataSenderMock;

        [SetUp]
        public void SetUp()
        {
            postDataBuilderMock = MockRepository.GenerateMock<IPostDataBuilder>();
            googleDataSenderMock = MockRepository.GenerateMock<IGoogleDataSender>();
            eventTracker = new EventTracker(postDataBuilderMock, googleDataSenderMock);

            //an event that has enough data populated that it could be logged successfully
            analyticsEvent = new UniversalAnalyticsEvent(postDataBuilderMock)
            {
                AnonymousClientId = "client id",
                Category = "category",
                Action = "action",
                Label = "label"
            };
        }

        [Test]
        public void ItSendsTheDataToGoogle()
        {
            string expectedPostData = "some amazing string that matches what google requires";
            
            postDataBuilderMock.Expect(mock => mock.BuildPostDataString(Arg<UniversalAnalyticsEvent>.Is.Anything))
                .Return(expectedPostData);

            eventTracker.TrackEvent(analyticsEvent);

            googleDataSenderMock.AssertWasCalled(mock => mock.SendData(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }
    }
}
