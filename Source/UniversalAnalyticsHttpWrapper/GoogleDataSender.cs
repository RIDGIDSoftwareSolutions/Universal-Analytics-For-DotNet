using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UniversalAnalyticsHttpWrapper
{
    internal class GoogleDataSender : IGoogleDataSender
    {
        public void SendData(Uri googleCollectionUri, string postData)
        {
            if (String.IsNullOrEmpty(postData))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            HttpWebRequest httpRequest = WebRequest.CreateHttp(googleCollectionUri);
            httpRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpRequest.Method = "POST";
            using(Stream requestStream = httpRequest.GetRequestStream())
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

        public async Task SendDataAsync(Uri googleCollectionUri, string postData)
        {
            if (String.IsNullOrEmpty(postData))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            HttpWebRequest httpRequest = WebRequest.CreateHttp(googleCollectionUri);
            httpRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpRequest.Method = "POST";
            using (Stream requestStream = await httpRequest.GetRequestStreamAsync())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    await writer.WriteAsync(postData);
                }
            }

            using (var webResponse = (HttpWebResponse)await httpRequest.GetResponseAsync())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
            }
        }
    }
}
