using System;
using NServiceBus;
using NServiceBus.Transport;

public class CustomizedRecoverabilityPolicy
{
    public static RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
    {
        // Early decisions and return before custom policy is invoked
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

        // Override delayed retry decision for custom exception
        // i.e. MyOtherBusinessException should do fixed backoff of 5 seconds
        if (context.Exception is MyOtherBusinessException &&
            context.DelayedDeliveriesPerformed < config.Delayed.MaxNumberOfRetries)
        {
            return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
        }

        // in all other cases No Immediate or Delayed Retries, go to default error queue
        return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
    }
}
