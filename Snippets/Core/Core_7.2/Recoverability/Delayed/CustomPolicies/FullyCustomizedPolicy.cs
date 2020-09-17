namespace Core7.Recoverability.Delayed.CustomPolicies
{
    using System;
    using NServiceBus;
    using NServiceBus.Transport;

    class FullyCustomizedPolicy
    {
        FullyCustomizedPolicy(EndpointConfiguration endpointConfiguration)
        {
            #region FullyCustomizedPolicyRecoverabilityConfigurationWithDiscard

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.AddUnrecoverableException<MyBusinessException>();
            recoverability.CustomPolicy(MyCustomRetryPolicy);
            // configuration can be changed at this point, data will be passed to the policy
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

        #region FullyCustomizedPolicyWithDiscard

        RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            // early decisions and return before custom policy is invoked
            // i.e. all unrecoverable exceptions should always go to customized error queue
            foreach (var exceptionType in config.Failed.UnrecoverableExceptionTypes)
            {
                if (exceptionType.IsInstanceOfType(context.Exception))
                {
                    return RecoverabilityAction.MoveToError("customErrorQueue");
                }
            }

            // If it does not make sense to have this message around anymore
            // it can be discarded with a reason.
            if (context.Exception is MyBusinessTimedOutException)
            {
                return RecoverabilityAction.Discard("Business operation timed out.");
            }

            // override delayed retry decision for custom exception
            // i.e. MyOtherBusinessException should do fixed backoff of 5 seconds
            if (context.Exception is MyOtherBusinessException &&
                context.DelayedDeliveriesPerformed < config.Delayed.MaxNumberOfRetries)
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

        class MyBusinessTimedOutException :
            Exception
        {
        }
    }
}