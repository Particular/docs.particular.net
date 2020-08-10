namespace Core8.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class ExceptionPolicyOverridingDelayedRetriesPart
    {
        #region DelayedRetriesCustomExceptionPolicyHandler

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            if (!(action is DelayedRetry delayedRetryAction))
            {
                return action;
            }
            if (context.Exception is MyBusinessException)
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            }
            // Override default delivery delay.
            return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
        }

        #endregion

    }
}