using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IGoogleDataSender
    {
        void SendData(Uri googleCollectionUri, string postData);
        Task SendDataAsync(Uri googleCollectionUri, IEnumerable<KeyValuePair<string, string>> postData);
    }
}
