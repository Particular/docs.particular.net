// ReSharper disable UnusedParameter.Local
namespace Snippets6.BusNotifications
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
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            Subscribe(endpointConfiguration.Notifications);
        }

        void Subscribe(Notifications notifications)
        {
            ErrorsNotifications errors = notifications.Errors;
            errors.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => LogEvent(retry);
            errors.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => LogEvent(retry);
            errors.MessageSentToErrorQueue += (sender, retry) => LogEvent(retry);
        }

        void LogEvent(FailedMessage failedMessage)
        {
            log.Info("Mesage sent to error queue");
        }

        void LogEvent(SecondLevelRetry secondLevelRetry)
        {
            log.Info("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
        }

        void LogEvent(FirstLevelRetry firstLevelRetry)
        {
            log.Info("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
        }

        #endregion
    }

}