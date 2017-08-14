namespace Core3.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus.Management.Retries;
    using NServiceBus.Unicast.Transport;

    class SimplePolicy
    {
        SimplePolicy()
        {
            #region DelayedRetriesCustomPolicy

            SecondLevelRetries.RetryPolicy = MyCustomRetryPolicy;

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
            var headers = transportMessage.Headers;
            if (headers.TryGetValue(NServiceBus.Headers.Retries, out var value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        #endregion
    }
}