namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;

    class SimplePolicy
    {
        SimplePolicy(EndpointConfiguration endpointConfiguration)
        {
            #region SecondLevelRetriesCustomPolicy

            var retriesSettings = endpointConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler
        TimeSpan MyCustomRetryPolicy(SecondLevelRetryContext context)
        {
            // retry max 3 times
            if (context.SecondLevelRetryAttempt >= 3)
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