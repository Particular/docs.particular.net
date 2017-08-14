namespace Core4.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;

    class SimplePolicy
    {
        SimplePolicy()
        {
            #region DelayedRetriesCustomPolicy

            Configure.Features.SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));

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
            if (headers.TryGetValue(Headers.Retries, out var value))
            {
                return int.Parse(value);
            }
            return 0;
        }
        #endregion

    }
}