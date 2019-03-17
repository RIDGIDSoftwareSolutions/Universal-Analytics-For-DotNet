using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Interface for calls related to tracking events using the Universal Analytics Measurement Protocol.
    /// </summary>
    public interface IEventTracker
    {
        /// <summary>
        /// Tracks the event and puts the result in a container object.
        /// </summary>
        /// <param name="analyticsEvent"></param>
        /// <returns>The result of the tracking operation</returns>
        TrackingResult TrackEvent(IUniversalAnalyticsEvent analyticsEvent);
        
        /// <summary>
        /// Tracks the event and puts the result in a container object.
        /// </summary>
        /// <param name="analyticsEvent"></param>
        /// <returns>The result of the tracking operation</returns>
        Task<TrackingResult> TrackEventAsync(IUniversalAnalyticsEvent analyticsEvent);


        /// <summary>
        /// Adds "raw" (custom) payload to the hit data.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters for parameters list. For example AddToCustomPayload("aip", "1") to enable IP anonymization.
        /// Only use this for fields not directly supported by this wrapper. will throw ArgumentException otherwise.
        /// If parameter was already added, it's value will be replaced with the supplied one
        /// </summary>
        /// <param name="name">Google Analytics Measurement Protocol short parameter name. for ex: aip, ds, qt, etc</param>
        /// <param name="value">Parameter value</param>
        void AddToCustomPayload(string name, string value);
    }
}
