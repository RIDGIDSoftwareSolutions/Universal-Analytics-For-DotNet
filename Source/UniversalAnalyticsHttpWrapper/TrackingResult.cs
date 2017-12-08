using System;
using System.Linq;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Result of a tracking request.
    /// </summary>
    public class TrackingResult
    {
        /// <summary>
        /// Creates a tracking result based on the HTTP status code as well as an exception, if any.
        /// </summary>
        /// <param name="exception"></param>
        public TrackingResult(Exception exception = null)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Checks if the tracking attempt was successful.
        /// </summary>
        public bool Failed => Exception != null;
        /// <summary>
        /// An exception caught during the tracking process. Not thrown for stability.
        /// </summary>
        public Exception Exception { get; internal set; }
    }
}
