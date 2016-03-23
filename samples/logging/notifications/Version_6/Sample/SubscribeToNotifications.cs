using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Logging;

// ReSharper disable UnusedParameter.Local

#region subscriptions

public class SubscribeToNotifications :
    IWantToRunWhenBusStartsAndStops
{
    static ILog log = LogManager.GetLogger<SubscribeToNotifications>();
    Notifications notifications;

    public SubscribeToNotifications(Notifications notifications)
    {
        this.notifications = notifications;
    }

    public Task Start(IMessageSession session)
    {
        ErrorsNotifications errors = notifications.Errors;
        errors.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => Log(retry);
        errors.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => Log(retry);
        errors.MessageSentToErrorQueue += (sender, retry) => Log(retry);
        return Task.FromResult(0);
    }

    void Log(FailedMessage failedMessage)
    {
        log.Info("Mesage sent to error queue");
    }

    void Log(SecondLevelRetry secondLevelRetry)
    {
        log.Info("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
    }

    void Log(FirstLevelRetry firstLevelRetry)
    {
        log.Info("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
    }

    public Task Stop(IMessageSession session)
    {
        return Task.FromResult(0);
    }

}

#endregion