﻿namespace Core4.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;

    class ExceptionPolicy
    {

        ExceptionPolicy()
        {
            Configure.Features.SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));
        }

        #region SecondLevelRetriesCustomExceptionPolicyHandler

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
            string value;
            var headers = transportMessage.Headers;
            if (headers.TryGetValue(Headers.Retries, out value))
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