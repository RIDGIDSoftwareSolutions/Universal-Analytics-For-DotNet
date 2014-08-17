using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void SendAnEventToASampleAnalyticsProperty()
        {
            EventTracker eventTracker = new EventTracker();
            UniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
                "developer", 
                "test category", 
                "test action", 
                "test label", 
                "10");
            eventTracker.TrackEvent(analyticsEvent);
        }
    }
}
