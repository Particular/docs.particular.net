// ReSharper disable UnusedParameter.Local
namespace Core6.Notifications
{
    using System.Text;
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

        static string GetMessageString(byte[] body)
        {
            return Encoding.UTF8.GetString(body);
        }

        void LogEvent(object sender, FailedMessage failed)
        {
            log.Fatal($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
        }

        void LogEvent(object sender, DelayedRetryMessage retry)
        {
            log.Info($@"Message sent to Delayed Retries.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
        }

        void LogEvent(object sender, ImmediateRetryMessage retry)
        {
            log.Info($@"Message sent to Immedediate Retry.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
        }

        #endregion
    }

}