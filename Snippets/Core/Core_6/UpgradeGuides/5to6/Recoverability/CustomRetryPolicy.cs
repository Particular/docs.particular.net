namespace Core6.UpgradeGuides._5to6.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    static class SimplePolicy
    {
        static SimplePolicy()
        {
            EndpointConfiguration endpointConfiguration = null;

            #region 5to6-DelayedRetriesCustomPolicy

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    // desired number of retries
                    delayed.NumberOfRetries(3);
                });
            recoverability.CustomPolicy(MyCustomRetryPolicy);

            #endregion
        }


        #region 5to6-DelayedRetriesCustomPolicyHandler

        static RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var numberOfRetries = context.DelayedDeliveriesPerformed;
            var exceptionInstance = context.Exception;

            // call the default recoverability of default behavior is desired
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            if (action is DelayedRetry delayedRetryAction)
            {
                // perform some logic and decide when to do delayed retries
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }

        #endregion
    }

}