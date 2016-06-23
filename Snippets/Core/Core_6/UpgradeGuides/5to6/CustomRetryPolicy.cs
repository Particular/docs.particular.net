namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    static class SimplePolicy
    {
        static SimplePolicy()
        {
            EndpointConfiguration endpointConfiguration = null;

            #region 5to6-SecondLevelRetriesCustomPolicy

            var retriesSettings = endpointConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }


        #region 5to6-SecondLevelRetriesCustomPolicyHandler

        static TimeSpan MyCustomRetryPolicy(SecondLevelRetryContext context)
        {
            var numberOfRetries = context.SecondLevelRetryAttempt;
            var exceptionInstance = context.Exception;

            // perform some logic and decide when to retry
            return TimeSpan.FromSeconds(5);
        }

        #endregion
    }

}

