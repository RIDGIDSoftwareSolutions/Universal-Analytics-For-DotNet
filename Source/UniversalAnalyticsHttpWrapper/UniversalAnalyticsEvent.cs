using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    public class UniversalAnalyticsEvent
    {
        internal string anonymousClientId;
        internal string eventCategory;
        internal string eventAction;
        internal string eventLabel;
        internal string eventValue;

        public UniversalAnalyticsEvent(
            string anonymousClientId,
            string eventCategory,
            string eventAction,
            string eventLabel = null,
            string eventValue = null)
        {
            this.anonymousClientId = anonymousClientId;
            this.eventCategory = eventCategory;
            this.eventAction = eventAction;
            this.eventLabel = eventLabel;
            this.eventValue = eventValue;
        }

        public UniversalAnalyticsEvent(
            PostDataBuilder postDataBuilder,
            string anonymousClientId,
            string eventCategory,
            string eventAction,
            string eventLabel = null,
            string eventValue = null)
        {
            this.anonymousClientId = anonymousClientId;
            this.eventCategory = eventCategory;
            this.eventAction = eventAction;
            this.eventLabel = eventLabel;
            this.eventValue = eventValue;
        }

        public string AnonymousClientId { get { return this.anonymousClientId; } }
        public string EventCategory { get { return this.eventCategory; } }
        public string EventAction { get { return this.eventAction; } }
        public string EventLabel { get { return this.eventLabel; } }
        public string EventValue { get { return this.eventValue; } }
    }
}
