using System.Configuration;

namespace UniversalAnalyticsHttpWrapper.Exceptions
{
    internal class ConfigEntryMissingException : ConfigurationErrorsException
    {
        internal const string EXCEPTION_MESSAGE_FORMAT = "No app setting could be found for '{0}'.";

        private readonly string appKeyForMissingSetting;

        internal ConfigEntryMissingException(string appKeyForMissingSetting)
        {
            this.appKeyForMissingSetting = appKeyForMissingSetting;
        }

        public override string Message => string.Format(EXCEPTION_MESSAGE_FORMAT, appKeyForMissingSetting);
    }
}
