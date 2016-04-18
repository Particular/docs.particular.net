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
        ErrorsNotifications errors = endpointConfiguration.Notifications.Errors;
        errors.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => Log(retry);
        errors.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => Log(retry);
        errors.MessageSentToErrorQueue += (sender, retry) => Log(retry);
    }

    static void Log(FailedMessage failedMessage)
    {
        log.Fatal("Mesage sent to error queue");
    }

    static void Log(SecondLevelRetry secondLevelRetry)
    {
        log.Fatal("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
    }

    static void Log(FirstLevelRetry firstLevelRetry)
    {
        log.Fatal("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
    }

}

#endregion