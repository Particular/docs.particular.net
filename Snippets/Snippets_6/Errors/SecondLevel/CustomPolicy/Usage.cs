namespace Snippets6.Errors.SecondLevel.CustomPolicy
{
    using System;
    using NServiceBus;
    using NServiceBus.SecondLevelRetries.Config;
    using NServiceBus.Transports;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region SecondLevelRetriesCustomPolicy

            SecondLevelRetriesSettings retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler
        TimeSpan MyCustomRetryPolicy(IncomingMessage message)
        {
            // retry max 3 times
            if (GetNumberOfRetries(message) >= 3)
            {
                // sending back a TimeSpan.MinValue tells the 
                // SecondLevelRetry not to retry this message
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }

        static int GetNumberOfRetries(IncomingMessage message)
        {
            string value;
            if (message.Headers.TryGetValue(Headers.Retries, out value))
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion

    }
}