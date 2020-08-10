namespace Core8.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class PartiallyCustomizedPolicy
    {
        PartiallyCustomizedPolicy(EndpointConfiguration endpointConfiguration)
        {
            #region PartiallyCustomizedPolicyRecoverabilityConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.AddUnrecoverableException<MyBusinessException>();
            recoverability.CustomPolicy(MyCustomRetryPolicy);
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(3);
                });
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(3).TimeIncrease(TimeSpan.FromSeconds(2));
                });

            #endregion
        }

        #region PartiallyCustomizedPolicy

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            // invocation of default recoverability policy
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            // override delayed retry decision for custom exception
            // i.e. MyOtherBusinessException should do fixed backoff of 5 seconds
            if (action is DelayedRetry delayedRetryAction &&
                context.Exception is MyOtherBusinessException)
            {
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            return action;
        }

        #endregion

        class MyOtherBusinessException :
            Exception
        {
        }
    }
}