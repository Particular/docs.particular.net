namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    static class SimplePolicy
    {
        static SimplePolicy()
        {
            EndpointConfiguration endpointConfiguration = null;

            #region 5to6-SecondLevelRetriesCustomPolicy

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Delayed(delayed => delayed.NumberOfRetries(3)); // desired number of retries
            recoverabilitySettings.CustomPolicy(MyCustomRetryPolicy);

            #endregion
        }


        #region 5to6-SecondLevelRetriesCustomPolicyHandler

        static RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var numberOfRetries = context.DelayedDeliveriesPerformed;
            var exceptionInstance = context.Exception;

            // call the default recoverability of default behavior is desired
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            var delayedRetryAction = action as DelayedRetry;
            if (delayedRetryAction != null)
            {
                // perform some logic and decide when to do delayed retries
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }

        #endregion
    }

}
