namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class PartiallyCustomizedPolicy
    {
        PartiallyCustomizedPolicy(EndpointConfiguration endpointConfiguration)
        {
            #region PartiallyCustomizedPolicyRecoverabilityConfiguration
            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.CustomPolicy(MyCustomRetryPolicy);
            recoverabilitySettings.Immediate(immediate => immediate.NumberOfRetries(3));
            recoverabilitySettings.Delayed(delayed => delayed.NumberOfRetries(3).TimeIncrease(TimeSpan.FromSeconds(2)));
            #endregion
        }

        #region PartiallyCustomizedPolicy
        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            // early decisions and return before custom policy is invoked
            // i.ex. MyBusinessException should always go to error
            if (context.Exception is MyBusinessException)
            {
                return RecoverabilityAction.MoveToError();
            }

            // invocation of default recoverability policy
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            // override delayed retry decision for custom exception
            // i.ex. MyOtherBusinessException should do fixed backoff of 5 seconds
            var delayedRetryAction = action as DelayedRetry;
            if (delayedRetryAction != null && context.Exception is MyOtherBusinessException)
            {
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }
        #endregion

        class MyOtherBusinessException : Exception { }
    }
}