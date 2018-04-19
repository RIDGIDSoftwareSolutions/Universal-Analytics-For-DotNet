using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IGoogleDataSender
    {
        void SendData(Uri googleCollectionUri, string postData);
        Task SendDataAsync(Uri googleCollectionUri, IEnumerable<KeyValuePair<string, string>> postData);
    }
}
