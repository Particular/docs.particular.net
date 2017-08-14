namespace Core5.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;

    public class ExceptionPolicy
    {

        ExceptionPolicy(BusConfiguration busConfiguration)
        {
            var retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);
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
            if (headers.TryGetValue(Headers.Retries, out var value))
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