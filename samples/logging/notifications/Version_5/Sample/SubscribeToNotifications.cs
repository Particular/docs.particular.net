using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using NServiceBus;
using NServiceBus.Faults;

#region subscriptions
public class SubscribeToNotifications : 
    IWantToRunWhenBusStartsAndStops, 
    IDisposable
{
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
                .Subscribe(LogToConsole)
            );
        unsubscribeStreams.Add(
            errors.MessageHasBeenSentToSecondLevelRetries
                .ObserveOn(scheduler) 
                .Subscribe(LogToConsole)
            );
        unsubscribeStreams.Add(
            errors.MessageHasFailedAFirstLevelRetryAttempt
                .ObserveOn(scheduler) 
                .Subscribe(LogToConsole)
            );
    }

    void LogToConsole(FailedMessage failedMessage)
    {
        Console.WriteLine("Mesage sent to error queue");
    }

    void LogToConsole(SecondLevelRetry secondLevelRetry)
    {
        Console.WriteLine("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
    }

    void LogToConsole(FirstLevelRetry firstLevelRetry)
    {
        Console.WriteLine("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
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