namespace Snippets6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus.Transports;

    class ExceptionPolicy
    {

        #region SecondLevelRetriesCustomExceptionPolicyHandler
        TimeSpan MyCustomRetryPolicy(IncomingMessage incomingMessage)
        {
            if (incomingMessage.ExceptionType() == typeof(MyBusinessException).FullName)
            {
                // Do not retry for MyBusinessException
                return TimeSpan.MinValue;
            }

            if (incomingMessage.NumberOfRetries() >= 3)
            {
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }


        #endregion

    }

    internal class MyBusinessException
    {
    }
}