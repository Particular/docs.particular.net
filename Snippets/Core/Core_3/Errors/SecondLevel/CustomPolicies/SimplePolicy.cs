﻿namespace Core3.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus.Management.Retries;
    using NServiceBus.Unicast.Transport;

    class SimplePolicy
    {
        SimplePolicy()
        {
            #region SecondLevelRetriesCustomPolicy

            SecondLevelRetries.RetryPolicy = MyCustomRetryPolicy;

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler

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
            var headers = transportMessage.Headers;
            if (headers.TryGetValue(NServiceBus.Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        #endregion
    }
}