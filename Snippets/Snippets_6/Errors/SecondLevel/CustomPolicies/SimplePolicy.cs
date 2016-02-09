namespace Snippets6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.SecondLevelRetries.Config;
    using NServiceBus.Transports;

    public class SimplePolicy
    {
        public SimplePolicy()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();
            #region SecondLevelRetriesCustomPolicy

            SecondLevelRetriesSettings retriesSettings = configuration.SecondLevelRetries();
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