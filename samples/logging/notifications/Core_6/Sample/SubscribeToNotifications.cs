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

    static void Log(FailedMessage failedMessage)
    {
        log.Fatal("Message sent to error queue");
    }

    static void Log(DelayedRetryMessage secondLevelRetry)
    {
        log.Fatal($"Message sent to SLR. RetryAttempt:{secondLevelRetry.RetryAttempt}");
    }

    static void Log(ImmediateRetryMessage firstLevelRetry)
    {
        log.Fatal($"Message sent to FLR. RetryAttempt:{firstLevelRetry.RetryAttempt}");
    }

}

#endregion