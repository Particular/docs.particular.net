// ReSharper disable UnusedParameter.Local

namespace Core8.Notifications
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class SubscribeToNotifications
    {
        static ILog log = LogManager.GetLogger<SubscribeToNotifications>();

        void EndpointStartup(EndpointConfiguration endpointConfiguration)
        {
            #region SubscribeToErrorsNotifications
            var recoverability = endpointConfiguration.Recoverability();

            recoverability.Immediate(settings => settings.OnMessageBeingRetried((retry, ct) =>
            {
                log.Info($"Message {retry.MessageId} will be retried immediately.");
                return Task.CompletedTask;
            }));

            recoverability.Delayed(settings => settings.OnMessageBeingRetried((retry, ct) =>
            {
                log.Info($@"Message {retry.MessageId} will be retried after a delay.");
                return Task.CompletedTask;
            }));

            recoverability.Failed(settings => settings.OnMessageSentToErrorQueue((failed, ct) =>
            {
                log.Fatal($@"Message {failed.MessageId} will be sent to the error queue.");
                return Task.CompletedTask;
            }));
            #endregion
        }

    }

}