using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace UniversalAnalyticsHttpWrapper
{
    internal class GoogleDataSender : IGoogleDataSender
    {
        public void SendData(Uri googleCollectionUri, string postData)
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp(googleCollectionUri);
            httpRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpRequest.Method = "POST";
            using(Stream requestStream = httpRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(httpRequest.GetRequestStream()))
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
    }
}
