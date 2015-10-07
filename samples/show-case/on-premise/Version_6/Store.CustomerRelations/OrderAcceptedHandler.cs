namespace Store.CustomerRelations
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        IBus bus;

        public OrderAcceptedHandler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(OrderAccepted message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Console.WriteLine("Customer: {0} is now a preferred customer publishing for other service concerns", message.ClientId);

            // publish this event as an asynchronous event
            await bus.PublishAsync<ClientBecamePreferred>(m =>
            {
                m.ClientId = message.ClientId;
                m.PreferredStatusExpiresOn = DateTime.Now.AddMonths(2);
            });
        }
    }
}
