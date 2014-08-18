Universal-Analytics-For-DotNet
==============================

A .NET wrapper over top of Google's Universal Analytics Measurement Protocol HTTP API. For now, this wrapper allows you to push Events from server-side code. This offers the advantages of 1) not relying on client-side javascript and 2) allowing you to push more interesting events that may not be available on the client side. For example, if you have a website for collecting donations you could push an event to indicate that a donation occurred and push the "value" of that donation. See https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide#event for more details.

Pushing an event is as simple as:

```
EventTracker eventTracker = new EventTracker();
UniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
    //Anonymous client id. See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
    "developer",
    "test category", 
    "test action", 
    "test label", 
    "10");
eventTracker.TrackEvent(analyticsEvent);
```
