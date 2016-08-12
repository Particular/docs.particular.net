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
            errors.MessageHasBeenSentToDelayedRetries += LogEvent;
            errors.MessageHasFailedAnImmediateRetryAttempt += LogEvent;
            errors.MessageSentToErrorQueue += LogEvent;
        }

        void LogEvent(object sender, FailedMessage failedMessage)
        {
            log.Info("Message sent to error queue");
        }

        void LogEvent(object sender, DelayedRetryMessage e)
        {
            log.Info($"Message sent to Delayed Retries. RetryAttempt:{e.RetryAttempt}");
        }

        void LogEvent(object sender, ImmediateRetryMessage e)
        {
            log.Info($"Message has failed an immedediate retry attempt. RetryAttempt:{e.RetryAttempt}");
        }

        #endregion
    }

}