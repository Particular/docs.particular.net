﻿namespace Core6.Recoverability.Delayed.CustomPolicies
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
            var delayedRetryAction = action as DelayedRetry;
            if (delayedRetryAction != null)
            {
                // Override default delivery delay.
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }

        #endregion
    }
}