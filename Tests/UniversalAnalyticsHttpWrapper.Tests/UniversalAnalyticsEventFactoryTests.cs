using NUnit.Framework;
using Rhino.Mocks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventFactoryTests
    {
        private IConfigurationManager configurationManagerMock;
        UniversalAnalyticsEventFactory factory;
        private string trackingId = "tracking id";
        private string anonymousClientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";

        [SetUp]
        public void SetUp()
        {
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();

             factory = new UniversalAnalyticsEventFactory(configurationManagerMock);
        }

        [Test]
        public void ItReturnsANewUniversalAnalyticsEvent()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(anonymousClientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }
    }
}
