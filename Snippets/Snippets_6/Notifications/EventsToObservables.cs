using System;

namespace Snippets6.Notifications
{
    using System.Reactive;
    using System.Reactive.Linq;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    class EventsToObservables
    {
        EventsToObservables(Notifications notifications, ILog log)
        {
            #region ConvertEventToObservable

            IObservable<EventPattern<FailedMessage>> failedMessages = Observable.FromEventPattern<EventHandler<FailedMessage>, FailedMessage>(
                handler => notifications.Errors.MessageSentToErrorQueue += handler,
                handler => notifications.Errors.MessageSentToErrorQueue -= handler);

            IDisposable subscription = failedMessages
                .Subscribe(x =>
                {
                    FailedMessage failedMessage = x.EventArgs;
                    log.Error($"Message {failedMessage.MessageId} moved to error queue", failedMessage.Exception);
                });

            #endregion
        }
    }
}
