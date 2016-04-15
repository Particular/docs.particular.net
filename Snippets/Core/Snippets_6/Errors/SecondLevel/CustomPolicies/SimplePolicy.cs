namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.SecondLevelRetries.Config;
    using NServiceBus.Transports;

    class SimplePolicy
    {
        SimplePolicy(EndpointConfiguration endpointConfiguration)
        {
            #region SecondLevelRetriesCustomPolicy

            SecondLevelRetriesSettings retriesSettings = endpointConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler
        TimeSpan MyCustomRetryPolicy(IncomingMessage incomingMessage)
        {
            // retry max 3 times
            if (incomingMessage.NumberOfRetries() >= 3)
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