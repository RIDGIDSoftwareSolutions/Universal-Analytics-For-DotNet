using System;
namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Represents a Universal Analytics event. See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#events
    /// for more details on the primary fields for an event.
    /// </summary>
    public interface IUniversalAnalyticsEvent
    {
        /// <summary>
        /// Gets the tracking id this event.
        /// </summary>
        string TrackingId { get; }
        /// <summary>
        /// Gets the anonymous client Id for this event.
        /// </summary>
        string AnonymousClientId { get; }
        /// <summary>
        /// Gets the event action for this event.
        /// </summary>
        string EventAction { get; }
        /// <summary>
        /// Gets the event category for this event.
        /// </summary>
        string EventCategory { get; }
        /// <summary>
        /// Gets the event label for this event.
        /// </summary>
        string EventLabel { get; }
        /// <summary>
        /// Gets the event value for this event.
        /// </summary>
        string EventValue { get; }
        /// <summary>
        /// Gets the optional user Id for this event.
        /// </summary>
        string UserId { get; }
    }
}
