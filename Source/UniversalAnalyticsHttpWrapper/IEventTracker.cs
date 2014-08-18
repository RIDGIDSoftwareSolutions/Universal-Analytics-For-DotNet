using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Interface for calls related to tracking events using the Universal Analytics Measurement Protocol.
    /// </summary>
    public interface IEventTracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticsEvent"></param>
        void TrackEvent(IUniversalAnalyticsEvent analyticsEvent);
    }
}
