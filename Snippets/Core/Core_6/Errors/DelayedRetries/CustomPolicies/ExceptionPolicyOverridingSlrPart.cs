namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class ExceptionPolicyOverridingSlrPart
    {
        ExceptionPolicyOverridingSlrPart(EndpointConfiguration endpointConfiguration)
        {
            var recoverabilitySettings = endpointConfiguration.Recoverability();

            #region SecondLevelRetriesCustomExceptionPolicyHandlerConfig 6

            recoverabilitySettings.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(3);
                });

            #endregion

            recoverabilitySettings.CustomPolicy(MyCustomRetryPolicy);
        }

        #region SecondLevelRetriesCustomExceptionPolicyHandler

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

    class MyBusinessException : Exception
    {
    }
}