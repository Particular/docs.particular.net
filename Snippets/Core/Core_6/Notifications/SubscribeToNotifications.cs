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
            var errorsNotifications = notifications.Errors;
            errorsNotifications.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => LogEvent(retry);
            errorsNotifications.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => LogEvent(retry);
            errorsNotifications.MessageSentToErrorQueue += (sender, retry) => LogEvent(retry);
        }

        void LogEvent(FailedMessage failedMessage)
        {
            log.Info("Message sent to error queue");
        }

        void LogEvent(SecondLevelRetry secondLevelRetry)
        {
            log.Info($"Message sent to SLR. RetryAttempt:{secondLevelRetry.RetryAttempt}");
        }

        void LogEvent(FirstLevelRetry firstLevelRetry)
        {
            log.Info($"Message sent to FLR. RetryAttempt:{firstLevelRetry.RetryAttempt}");
        }

        #endregion
    }

}