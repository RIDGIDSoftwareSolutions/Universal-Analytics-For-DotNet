using System.Collections.Generic;
using System.Collections.Specialized;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IPostDataBuilder
    {
        string BuildPostDataString(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent);
        IEnumerable<KeyValuePair<string, string>> BuildPostDataCollection(string measurementProtocolVersion, IUniversalAnalyticsEvent analyticsEvent, NameValueCollection customPayload = null);

        /// <summary>
        /// List of Google Analytics Measurement Protocol Parameters which are currently supported
        /// </summary>
        string[] SupportedParameters { get; }
    }
}
