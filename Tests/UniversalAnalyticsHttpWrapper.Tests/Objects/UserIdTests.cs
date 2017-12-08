using System;
using System.Linq;
using NUnit.Framework;
using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper.Tests.Objects
{
    [TestFixture]
    public class UserIdTests
    {
        [Test]
        public void ItUsesAPredefinedString()
        {
            string user = "user1";
            Assert.AreEqual(user, new UserId(user).Id);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ItThrowsForANullUserIdString()
        {
            string user = null;
            Assert.AreEqual(user, new UserId(user).Id);
        }
    }
}
