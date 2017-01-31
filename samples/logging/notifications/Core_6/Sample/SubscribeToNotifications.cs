using System.Text;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Logging;

// ReSharper disable UnusedParameter.Local

#region subscriptions

public static class SubscribeToNotifications
{
    static ILog log = LogManager.GetLogger(typeof(SubscribeToNotifications));

    public static void Subscribe(EndpointConfiguration endpointConfiguration)
    {
        var errors = endpointConfiguration.Notifications.Errors;
        errors.MessageHasBeenSentToDelayedRetries += (sender, retry) => Log(retry);
        errors.MessageHasFailedAnImmediateRetryAttempt += (sender, retry) => Log(retry);
        errors.MessageSentToErrorQueue += (sender, retry) => Log(retry);
    }

    static string GetMessageString(byte[] body)
    {
        return Encoding.UTF8.GetString(body);
    }

    static void Log(FailedMessage failed)
    {
        log.Fatal($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
    }

    static void Log(DelayedRetryMessage delayedRetry)
    {
        log.Fatal($@"Message sent to Delayed Retries.
        RetryAttempt:{delayedRetry.RetryAttempt}
        Body:
        {GetMessageString(delayedRetry.Body)}");
    }

    static void Log(ImmediateRetryMessage immediateRetry)
    {
        log.Fatal($@"Message sent to Immedediate Retry.
        RetryAttempt:{immediateRetry.RetryAttempt}
        Body:
        {GetMessageString(immediateRetry.Body)}");
    }

}

#endregion
