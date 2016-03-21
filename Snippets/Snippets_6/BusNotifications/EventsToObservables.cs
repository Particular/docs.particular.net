using System;

namespace Snippets6.BusNotifications
{
    using System.Reactive;
    using System.Reactive.Linq;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    class EventsToObservables
    {
        EventsToObservables(BusNotifications busNotifications, ILog log)
        {
            #region ConvertEventToObservable

            IObservable<EventPattern<FailedMessage>> failedMessages = Observable.FromEventPattern<EventHandler<FailedMessage>, FailedMessage>(
                handler => busNotifications.Errors.MessageSentToErrorQueue += handler,
                handler => busNotifications.Errors.MessageSentToErrorQueue -= handler);

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
