using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UniversalAnalyticsHttpWrapper
{
    internal class GoogleDataSender : IGoogleDataSender
    {
        private readonly HttpClient _httpClient;

        public GoogleDataSender()
        {
            _httpClient = new HttpClient();
        }

        public void SendData(Uri googleCollectionUri, string postData)
        {
            if (string.IsNullOrEmpty(postData))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            var httpRequest = WebRequest.CreateHttp(googleCollectionUri);
            httpRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpRequest.Method = "POST";

            using(var requestStream = httpRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(postData);
                }
            }
  
            using(var webResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
            }
        }

        public async Task SendDataAsync(Uri googleCollectionUri, IEnumerable<KeyValuePair<string, string>> postData)
        {
            if (postData == default(IEnumerable<KeyValuePair<string, string>>))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            var httpResponse = await _httpClient.PostAsync(googleCollectionUri, new FormUrlEncodedContent(postData));
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpException((int)httpResponse.StatusCode,
                    "Google Analytics tracking did not return OK 200");
            }
        }
    }
}
