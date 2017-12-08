using System;
using System.Linq;
using NUnit.Framework;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class TrackingResultTests
    {
        [Test]
        public void IfAnExceptionIsPresentTrackingResultFailedIsTrue()
        {
            var exception = new Exception("whoa!");
            var trackingResult = new TrackingResult(exception);

            Assert.True(trackingResult.Failed);
        }

        [Test]
        public void FailedIsFalseWhenTheExceptionIsNull()
        {
            var trackingResult = new TrackingResult(null);
            Assert.False(trackingResult.Failed);
        }
    }
}
