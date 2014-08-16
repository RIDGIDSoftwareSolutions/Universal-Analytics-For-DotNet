using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    public interface IPostDataBuilder
    {
        string BuildPostDataString(UniversalAnalyticsEvent analyticsEvent);
    }
}
