namespace Core3.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus.Management.Retries;
    using NServiceBus.Unicast.Transport;

    class ExceptionPolicy
    {
        ExceptionPolicy()
        {
            SecondLevelRetries.RetryPolicy = MyCustomRetryPolicy;
        }

        #region DelayedRetriesCustomExceptionPolicyHandler

        TimeSpan MyCustomRetryPolicy(TransportMessage transportMessage)
        {
            if (ExceptionType(transportMessage) == typeof(MyBusinessException).FullName)
            {
                // Do not retry for MyBusinessException
                return TimeSpan.MinValue;
            }

            if (NumberOfRetries(transportMessage) >= 3)
            {
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

        static string ExceptionType(TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            return headers["NServiceBus.ExceptionInfo.ExceptionType"];
        }

        #endregion

    }

    class MyBusinessException :
        Exception
    {
    }
}