namespace VideoStore.CustomerRelations
{
    using System;
    using System.Diagnostics;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        public IBus Bus { get; set; }
        public void Handle(OrderAccepted message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Console.WriteLine("Customer: {0} is now a preferred customer publishing for other service concerns", message.ClientId);

            // publish this event as an asynchronous event
            Bus.Publish<ClientBecamePreferred>(m =>
            {
                m.ClientId = message.ClientId;
                m.PreferredStatusExpiresOn = DateTime.Now.AddMonths(2);
            });
        }
    }
}
