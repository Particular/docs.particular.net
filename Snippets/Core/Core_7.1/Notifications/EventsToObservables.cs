namespace Core7.Notifications
{
    using System;
    using System.Reactive.Linq;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    class EventsToObservables
    {
        EventsToObservables(Notifications notifications, ILog log)
        {
            #region ConvertEventToObservable

            var errorsNotifications = notifications.Errors;
            var failedMessages = Observable.FromEventPattern<EventHandler<FailedMessage>, FailedMessage>(
                handler => errorsNotifications.MessageSentToErrorQueue += handler,
                handler => errorsNotifications.MessageSentToErrorQueue -= handler);

            var subscription = failedMessages
                .Subscribe(x =>
                {
                    var failedMessage = x.EventArgs;
                    log.Error($"Message {failedMessage.MessageId} moved to error queue", failedMessage.Exception);
                });

            #endregion
        }
    }
}
