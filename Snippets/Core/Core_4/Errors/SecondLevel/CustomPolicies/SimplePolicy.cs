namespace Core4.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;

    class SimplePolicy
    {
        SimplePolicy()
        {
            #region SecondLevelRetriesCustomPolicy

            Configure.Features.SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));

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
            if (headers.TryGetValue(Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }
        #endregion

    }
}