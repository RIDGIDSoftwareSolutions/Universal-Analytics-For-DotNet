using System;
namespace UniversalAnalyticsHttpWrapper
{
    public interface IUniversalAnalyticsEvent
    {
        string AnonymousClientId { get; }
        string EventAction { get; }
        string EventCategory { get; }
        string EventLabel { get; }
        string EventValue { get; }
        string MeasurementProtocolVersion { get; }
        string TrackingId { get; }
    }
}
