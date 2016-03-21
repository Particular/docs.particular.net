namespace Snippets5.Errors.SecondLevel.CustomPolicy
{
    using System;
    using NServiceBus;
    using NServiceBus.SecondLevelRetries.Config;

    class SimplePolicy
    {
        SimplePolicy(BusConfiguration busConfiguration)
        {
            #region SecondLevelRetriesCustomPolicy

            SecondLevelRetriesSettings retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler
        TimeSpan MyCustomRetryPolicy(TransportMessage transportMessage)
        {
            // retry max 3 times
            if (transportMessage.NumberOfRetries() >= 3)
            {
                // sending back a TimeSpan.MinValue tells the 
                // SecondLevelRetry not to retry this message
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }
        #endregion

    }
}