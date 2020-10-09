using NServiceBus;
using NServiceBus.Transport;

public class CustomizedRecoverabilityPolicy
{
    public static RecoverabilityAction MyCustomRetryPolicy(RecoverabilityConfig config, ErrorContext context)
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

        // in all other cases No Immediate or Delayed Retries, go to default error queue
        return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
    }
}
