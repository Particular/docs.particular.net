namespace Core5.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
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

        void LogEvent(FailedMessage failedMessage)
        {
            log.Info("Message sent to error queue");
        }

        void LogEvent(SecondLevelRetry retry)
        {
            log.Info($"Message sent to Delayed Reties. RetryAttempt:{retry.RetryAttempt}");
        }

        void LogEvent(FirstLevelRetry retry)
        {
            log.Info($"Message sent to Immediate Retries. RetryAttempt:{retry.RetryAttempt}");
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

