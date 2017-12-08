using System.Linq;
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
    }
}
