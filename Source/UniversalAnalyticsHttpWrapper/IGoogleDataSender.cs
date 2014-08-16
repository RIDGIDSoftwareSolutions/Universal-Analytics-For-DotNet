using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    public interface IGoogleDataSender
    {
        void SendData(Uri googleCollectionUri, string postData);
    }
}
