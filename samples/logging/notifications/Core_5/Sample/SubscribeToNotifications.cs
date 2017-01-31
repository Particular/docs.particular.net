using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
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
        var errors = busNotifications.Errors;
        var scheduler = Scheduler.Default;
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

    static string GetMessageString(byte[] body)
    {
        return Encoding.UTF8.GetString(body);
    }

    void Log(FailedMessage failed)
    {
        log.Fatal($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
    }

    void Log(SecondLevelRetry retry)
    {
        log.Fatal($@"Message sent to Delayed Retries.
        RetryAttempt: {retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
    }

    void Log(FirstLevelRetry retry)
    {
        log.Fatal($@"Message sent to Immediate Reties.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
    }

    public void Stop()
    {
        foreach (var unsubscribeStream in unsubscribeStreams)
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