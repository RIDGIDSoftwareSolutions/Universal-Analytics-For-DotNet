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
        public string Category { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
        public string AnonymousClientId { get; set; }
    }
}
