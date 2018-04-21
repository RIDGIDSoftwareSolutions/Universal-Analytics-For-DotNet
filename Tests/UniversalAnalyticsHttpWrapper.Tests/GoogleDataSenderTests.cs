using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class GoogleDataSenderTests
    {
        private GoogleDataSender _googleDataSender;
        private Uri _dataCollectionUri = EventTracker.GOOGLE_COLLECTION_URI;
        private IEnumerable<KeyValuePair<string, string>> _postDataCollection = new List<KeyValuePair<string, string>>();
        private string _postDataString = "the data being posted";

        [SetUp]
        public void SetUp()
        {
            _googleDataSender = new GoogleDataSender();
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
                _googleDataSender.SendData(_dataCollectionUri, _postDataString);
            });
        }

        [Test]
        public void ItSendsTheAsyncRequestWithoutThrowingExceptions()
        {
            Assert.DoesNotThrow(async () => {
                await _googleDataSender.SendDataAsync(_dataCollectionUri, _postDataCollection);
            });
        }

        [Test]
        public void ItThrowsExceptionIfRequestBodyEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => {
                _googleDataSender.SendData(_dataCollectionUri, string.Empty);
            });
        }

        [Test]
        public void ItThrowsExceptionIfAsyncRequestBodyEmpty()
        {
            Assert.Throws<ArgumentNullException>(async () => {
                await _googleDataSender.SendDataAsync(
                    _dataCollectionUri, default(IEnumerable<KeyValuePair<string, string>>));
            });
        }
    }
}
