using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IPostDataBuilder
    {
        string BuildPostDataString(IUniversalAnalyticsEvent analyticsEvent);
    }
}
