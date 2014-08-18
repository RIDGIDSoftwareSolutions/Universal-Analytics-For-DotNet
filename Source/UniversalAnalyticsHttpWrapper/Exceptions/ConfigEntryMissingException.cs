using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UniversalAnalyticsHttpWrapper.Exceptions
{
    internal class ConfigEntryMissingException : ConfigurationErrorsException
    {
        internal const string EXCEPTION_MESSAGE_FORMAT = "No app setting could be found for '{0}'.";

        private string appKeyForMissingSetting;

        internal ConfigEntryMissingException(string appKeyForMissingSetting)
        {
            this.appKeyForMissingSetting = appKeyForMissingSetting;
        }

        public override string Message
        {
            get
            {
                return string.Format(EXCEPTION_MESSAGE_FORMAT, appKeyForMissingSetting);
            }
        }
    }
}
