using System;

namespace UniversalAnalyticsHttpWrapper.Objects
{
    /// <summary>
    /// Data object holding a string representing a user id (or uid).
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid
    /// </summary>
    public class UserId
    {
        /// <summary>
        /// Creates a user id with an id of parameter 'id'.
        /// </summary>
        /// <param name="id">string representing the user id.</param>
        /// <exception cref="ArgumentNullException">Thrown if id is null.</exception>
        public UserId(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        /// <summary>
        /// The UA user Id.
        /// </summary>
        public string Id { get; }
    }
}
