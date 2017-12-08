using System;
using System.Linq;
using NUnit.Framework;
using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper.Tests.Objects
{
    [TestFixture]
    public class ClientIdTests
    {
        [Test]
        public void ItGeneratesAnIdIfOneIsNotSpecified()
        {
            Assert.IsNotNull(new ClientId().Id);
        }

        [Test]
        public void ItGeneratesDifferentIdsIfOneIsNotSpecified()
        {
            Assert.AreNotEqual(new ClientId().Id, new ClientId().Id);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ItThrowsForANullString()
        {
            string value = null;
            Assert.IsNotNull(new ClientId(value).Id);
        }

        [Test]
        public void ItGeneratesAnIdForAString()
        {
            Assert.IsNotNull(new ClientId("testClientId").Id);
        }

        [Test]
        public void ItGeneratesDifferentIdsForDifferentStrings()
        {
            Assert.AreNotEqual(new ClientId("testClientId1").Id, new ClientId("testClientId2").Id);
        }

        [Test]
        public void ItUsesAPredefinedGuid()
        {
            var expectedGuid = Guid.NewGuid();
            Assert.AreEqual(expectedGuid, new ClientId(expectedGuid).Id);
        }

        [Test]
        public void ItGeneratesEqualIdsForEqualStrings()
        {
            var testString = "testClientId";
            Assert.AreEqual(new ClientId(testString).Id, new ClientId(testString).Id);
        }
    }
}
