using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class GoogleDataSenderTests
    {
        private GoogleDataSender googleDataSender;
        private Uri dataCollectionUri = EventTracker.GOOGLE_COLLECTION_URI;
        private IEnumerable<KeyValuePair<string, string>> postDataCollection = new List<KeyValuePair<string, string>>();
        private string postDataString = "the data being posted";

        [SetUp]
        public void SetUp()
        {
            googleDataSender = new GoogleDataSender();
        }

        //[Test]
        //public void ItSetsTheContentLengthOnTheRequest() {
        //    googleDataSender.SendData(EventTracker.GOOGLE_COLLECTION_URI, postData);
        //    HttpWebRequest.CreateHtt
        //}

        [Test]
        public void ItSendsTheRequestWithoutThrowingExceptions()
        {
            Assert.DoesNotThrow(() => {
                googleDataSender.SendData(dataCollectionUri, postDataString);
            });
        }

        [Test]
        public void ItSendsTheAsyncRequestWithoutThrowingExceptions()
        {
            Assert.DoesNotThrow(async () => {
                await googleDataSender.SendDataAsync(dataCollectionUri, postDataCollection);
            });
        }

        [Test]
        public void ItThrowsExceptionIfRequestBodyEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => {
                googleDataSender.SendData(dataCollectionUri, string.Empty);
            });
        }

        [Test]
        public void ItThrowsExceptionIfAsyncRequestBodyEmpty()
        {
            Assert.Throws<ArgumentNullException>(async () => {
                await googleDataSender.SendDataAsync(
                    dataCollectionUri, default(IEnumerable<KeyValuePair<string, string>>));
            });
        }
    }
}
