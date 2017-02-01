namespace Core6.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class FullyCustomizedPolicy
    {
        FullyCustomizedPolicy(EndpointConfiguration endpointConfiguration)
        {
            #region FullyCustomizedPolicyRecoverabilityConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.CustomPolicy(MyCustomRetryPolicy);
            // configuration can still be tweaked on this level if desired, data will be passed into the policy
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(3);
                });
            recoverability.Delayed(
                delayed =>
                {
                    var retries = delayed.NumberOfRetries(3);
                    retries.TimeIncrease(TimeSpan.FromSeconds(2));
                });

            #endregion
        }

        #region FullyCustomizedPolicy

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            // early decisions and return before custom policy is invoked
            // i.e. MyBusinessException should always go to customized error queue
            if (context.Exception is MyBusinessException)
            {
                return RecoverabilityAction.MoveToError("customErrorQueue");
            }

            // override delayed retry decision for custom exception
            // i.e. MyOtherBusinessException should do fixed backoff of 5 seconds
            if (context.Exception is MyOtherBusinessException && context.DelayedDeliveriesPerformed <= config.Delayed.MaxNumberOfRetries)
            {
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
            }

            // in all other cases No Immediate or Delayed Retries, go to default error queue
            return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
        }

        #endregion

        class MyOtherBusinessException :
            Exception
        {
        }
    }
}