using System.Collections.Generic;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IPostDataBuilder
    {
        string BuildPostDataString(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent);
        IEnumerable<KeyValuePair<string, string>> BuildPostDataCollection(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent);
    }
}
