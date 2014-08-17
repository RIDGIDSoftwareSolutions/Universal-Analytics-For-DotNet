using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    public class UniversalAnalyticsEvent
    {
        private IPostDataBuilder postDataBuilder;

        public UniversalAnalyticsEvent()
        {
            this.postDataBuilder = new PostDataBuilder();
        }

        public UniversalAnalyticsEvent(IPostDataBuilder postDataBuilder)
        {
            this.postDataBuilder = postDataBuilder;
        }

        public string Version { get; set; }
        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventLabel { get; set; }
        public string AnonymousClientId { get; set; }
        public string EventValue { get; set; }
    }
}
