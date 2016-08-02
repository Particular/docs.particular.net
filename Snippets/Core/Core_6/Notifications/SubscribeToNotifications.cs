// ReSharper disable UnusedParameter.Local
namespace Core6.BusNotifications
{
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;


    class SubscribeToNotifications
    {
        static ILog log = LogManager.GetLogger<SubscribeToNotifications>();

        #region SubscribeToErrorsNotifications

        void EndpointStartup()
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            Subscribe(endpointConfiguration.Notifications);
        }

        void Subscribe(Notifications notifications)
        {
            var errors = notifications.Errors;
            errors.MessageHasBeenSentToSecondLevelRetries += LogEvent;
            errors.MessageHasFailedAFirstLevelRetryAttempt += LogEvent;
            errors.MessageSentToErrorQueue += LogEvent;
        }

        void LogEvent(object sender, FailedMessage failedMessage)
        {
            log.Info("Message sent to error queue");
        }

        void LogEvent(object sender, SecondLevelRetry retry)
        {
            log.Info($"Message sent to Delayed Retries. RetryAttempt:{retry.RetryAttempt}");
        }

        void LogEvent(object sender, FirstLevelRetry retry)
        {
            log.Info($"Message sent to Immediate Retries. RetryAttempt:{retry.RetryAttempt}");
        }

        #endregion
    }

}