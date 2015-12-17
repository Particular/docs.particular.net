using System;

namespace Snippets6.BusNotifications
{
    using System.Reactive;
    using System.Reactive.Linq;
    using NServiceBus;
    using NServiceBus.Faults;

    class EventsToObservables
    {
        void TransformEventToObservable()
        {
            BusNotifications busNotifications = new BusNotifications();

            #region ConvertEventToObservable

            IObservable<EventPattern<FailedMessage>> failedMessages = Observable.FromEventPattern<EventHandler<FailedMessage>, FailedMessage>(
                handler => busNotifications.Errors.MessageSentToErrorQueue += handler, 
                handler => busNotifications.Errors.MessageSentToErrorQueue -= handler);

            IDisposable subscription = failedMessages
                .Subscribe(x => Console.WriteLine($"Message {x.EventArgs.MessageId} moved to error queue"));

            #endregion

        }
    }
}
