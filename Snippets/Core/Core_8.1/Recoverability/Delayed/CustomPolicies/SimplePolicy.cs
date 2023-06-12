namespace Core8.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class SimplePolicy
    {
        SimplePolicy(EndpointConfiguration endpointConfiguration)
        {
            #region DelayedRetriesCustomPolicy

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.CustomPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region DelayedRetriesCustomPolicyHandler

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);
            if (!(action is DelayedRetry delayedRetryAction))
            {
                return action;
            }
            // Override default delivery delay.
            return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
        }

        #endregion
    }
}