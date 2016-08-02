namespace Core6.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class ExceptionPolicyOverridingDelayedRetriesPart
    {
        ExceptionPolicyOverridingDelayedRetriesPart(EndpointConfiguration endpointConfiguration)
        {

            #region DelayedRetriesCustomExceptionPolicyHandlerConfig 6

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(3);
                });

            #endregion

            recoverability.CustomPolicy(MyCustomRetryPolicy);
        }

        #region DelayedRetriesCustomExceptionPolicyHandler

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            var delayedRetryAction = action as DelayedRetry;
            if (delayedRetryAction != null)
            {
                if (context.Exception is MyBusinessException)
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                }
                // Override default delivery delay.
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }

        #endregion

    }
}