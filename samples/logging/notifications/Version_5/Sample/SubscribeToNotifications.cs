using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Logging;

#region subscriptions
public class SubscribeToNotifications : 
    IWantToRunWhenBusStartsAndStops, 
    IDisposable
{
    static ILog log = LogManager.GetLogger<SubscribeToNotifications>();
    BusNotifications busNotifications;
    List<IDisposable> unsubscribeStreams = new List<IDisposable>();

    public SubscribeToNotifications(BusNotifications busNotifications)
    {
        this.busNotifications = busNotifications;
    }

    public void Start()
    {
        ErrorsNotifications errors = busNotifications.Errors;
        DefaultScheduler scheduler = Scheduler.Default;
        unsubscribeStreams.Add(
            errors.MessageSentToErrorQueue
                .ObserveOn(scheduler) 
                .Subscribe(Log)
            );
        unsubscribeStreams.Add(
            errors.MessageHasBeenSentToSecondLevelRetries
                .ObserveOn(scheduler) 
                .Subscribe(Log)
            );
        unsubscribeStreams.Add(
            errors.MessageHasFailedAFirstLevelRetryAttempt
                .ObserveOn(scheduler) 
                .Subscribe(Log)
            );
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

    public void Stop()
    {
        foreach (IDisposable unsubscribeStream in unsubscribeStreams)
        {
            unsubscribeStream.Dispose();
        }
    }

    public void Dispose()
    {
        Stop();
    }
}
#endregion