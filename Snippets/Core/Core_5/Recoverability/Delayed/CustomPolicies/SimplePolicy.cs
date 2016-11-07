﻿namespace Core5.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;

    class SimplePolicy
    {
        SimplePolicy(BusConfiguration busConfiguration)
        {
            #region DelayedRetriesCustomPolicy

            var retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region DelayedRetriesCustomPolicyHandler

        TimeSpan MyCustomRetryPolicy(TransportMessage transportMessage)
        {
            // retry max 3 times
            if (NumberOfRetries(transportMessage) >= 3)
            {
                // sending back a TimeSpan.MinValue tells the
                // SecondLevelRetry not to retry this message
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }

        static int NumberOfRetries(TransportMessage transportMessage)
        {
            string value;
            message.Headers.TryGetValue(Headers.Retries, out value);

            int retries;
            int.TryParse(value, out retries);
            
            return retries;
        }

        #endregion

    }
}
