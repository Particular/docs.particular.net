﻿namespace Core5.Errors.SecondLevel.CustomPolicy
{
    using System;
    using NServiceBus;

    class SimplePolicy
    {
        SimplePolicy(BusConfiguration busConfiguration)
        {
            #region SecondLevelRetriesCustomPolicy

            var retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

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