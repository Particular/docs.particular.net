namespace Core5.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Text;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    #region SubscribeToErrorsNotifications

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
            var errorsNotifications = busNotifications.Errors;
            var defaultScheduler = Scheduler.Default;
            unsubscribeStreams.Add(
                errorsNotifications.MessageSentToErrorQueue
                    .ObserveOn(defaultScheduler)
                    .Subscribe(LogEvent)
                );
            unsubscribeStreams.Add(
                errorsNotifications.MessageHasBeenSentToSecondLevelRetries
                    .ObserveOn(defaultScheduler)
                    .Subscribe(LogEvent)
                );
            unsubscribeStreams.Add(
                errorsNotifications.MessageHasFailedAFirstLevelRetryAttempt
                    .ObserveOn(defaultScheduler)
                    .Subscribe(LogEvent)
                );
        }

        static string GetMessageString(byte[] body)
        {
            return Encoding.UTF8.GetString(body);
        }

        void LogEvent(FailedMessage failed)
        {
            log.Info($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
        }

        void LogEvent(SecondLevelRetry retry)
        {
            log.Info($@"Message sent to Delayed Retries.
        RetryAttempt: {retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
        }

        void LogEvent(FirstLevelRetry retry)
        {
            log.Info($@"Message sent to Immediate Reties.
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
}

