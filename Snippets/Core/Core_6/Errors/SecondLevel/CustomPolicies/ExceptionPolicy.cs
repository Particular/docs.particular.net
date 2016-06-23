namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;

    class ExceptionPolicy
    {
        ExceptionPolicy(EndpointConfiguration endpointConfiguration)
        {
            var retriesSettings = endpointConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);
        }

        #region SecondLevelRetriesCustomExceptionPolicyHandler
        TimeSpan MyCustomRetryPolicy(SecondLevelRetryContext context)
        {
            if (context.Exception is MyBusinessException)
            {
                // Do not retry for MyBusinessException
                return TimeSpan.MinValue;
            }

            if (context.SecondLevelRetryAttempt >= 3)
            {
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }


        #endregion

    }

    class MyBusinessException: Exception
    {
    }
}