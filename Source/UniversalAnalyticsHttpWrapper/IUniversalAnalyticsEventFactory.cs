using UniversalAnalyticsHttpWrapper.Objects;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Interface for making instances of IUniversalAnalyticsEvent objects
    /// </summary>
    public interface IUniversalAnalyticsEventFactory
    {
        /// <summary>
        /// This constructor expects an App Setting for 'UniversalAnalytics.TrackingId' 
        /// in the config. UniversalAnalytics.TrackingId must be a Universal Analytics Web Property.
        /// </summary>
        /// <param name="clientId">Required. Anonymous client id for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.</param>
        /// <param name="eventCategory">Required. The event category for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.</param>
        /// <param name="eventAction">Required. The event action for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.</param>
        /// <param name="eventLabel">The event label for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.</param>
        /// <param name="eventValue">Optional. The event value for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.</param>
        /// <param name="userId">Optional. The userId value for the event. This will override anonymousClientId.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.</param>
        /// <exception cref="UniversalAnalyticsHttpWrapper.Exceptions.ConfigEntryMissingException">Thrown when
        /// one of the required config attributes are missing.</exception>
        /// <exception cref="System.ArgumentException">Thrown when one of the required fields are null or whitespace.</exception>
        /// <exception cref="System.Web.HttpException">Thrown when the HttpRequest that's posted to Google returns something
        /// other than a 200 OK response.</exception>
        /// 
        IUniversalAnalyticsEvent MakeUniversalAnalyticsEvent(
            string clientId,
            string eventCategory,
            string eventAction,
            string eventLabel,
            string eventValue = null,
            string userId = null);

        /// <summary>
        /// This constructor expects an App Setting for 'UniversalAnalytics.TrackingId' 
        /// in the config. UniversalAnalytics.TrackingId must be a Universal Analytics Web Property.
        /// </summary>
        /// <param name="clientId">Required. Anonymous client id for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.</param>
        /// <param name="eventCategory">Required. The event category for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.</param>
        /// <param name="eventAction">Required. The event action for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.</param>
        /// <param name="eventLabel">The event label for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.</param>
        /// <param name="eventValue">Optional. The event value for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.</param>
        /// <exception cref="UniversalAnalyticsHttpWrapper.Exceptions.ConfigEntryMissingException">Thrown when
        /// one of the required config attributes are missing.</exception>
        /// <exception cref="System.ArgumentException">Thrown when one of the required fields are null or whitespace.</exception>
        /// <exception cref="System.Web.HttpException">Thrown when the HttpRequest that's posted to Google returns something
        /// other than a 200 OK response.</exception>
        IUniversalAnalyticsEvent MakeUniversalAnalyticsEvent(
            ClientId clientId,
            string eventCategory,
            string eventAction,
            string eventLabel,
            string eventValue = null);

        /// <summary>
        /// This constructor expects an App Setting for 'UniversalAnalytics.TrackingId' 
        /// in the config. UniversalAnalytics.TrackingId must be a Universal Analytics Web Property.
        /// </summary>
        /// <param name="userId">Required. The user id for the event. Will create an event with the uid defined.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.</param>
        /// <param name="eventCategory">Required. The event category for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.</param>
        /// <param name="eventAction">Required. The event action for the event. 
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.</param>
        /// <param name="eventLabel">The event label for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.</param>
        /// <param name="eventValue">Optional. The event value for the event.
        /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.</param>
        /// <exception cref="UniversalAnalyticsHttpWrapper.Exceptions.ConfigEntryMissingException">Thrown when
        /// one of the required config attributes are missing.</exception>
        /// <exception cref="System.ArgumentException">Thrown when one of the required fields are null or whitespace.</exception>
        /// <exception cref="System.Web.HttpException">Thrown when the HttpRequest that's posted to Google returns something
        /// other than a 200 OK response.</exception>
        IUniversalAnalyticsEvent MakeUniversalAnalyticsEvent(
            UserId userId,
            string eventCategory,
            string eventAction,
            string eventLabel,
            string eventValue = null);
    }
}
