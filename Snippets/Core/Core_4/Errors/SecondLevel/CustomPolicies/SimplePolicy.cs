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
            if (transportMessage.NumberOfRetries() >= 3)
            {
                // sending back a TimeSpan.MinValue tells the
                // SecondLevelRetry not to retry this message
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }
        #endregion

    }
}