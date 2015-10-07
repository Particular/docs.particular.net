namespace Store.CustomerRelations
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class SendLimitedTimeOffer : IHandleMessages<ClientBecamePreferred>
    {
        public Task Handle(ClientBecamePreferred message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }
            Console.WriteLine("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {0}", message.ClientId);
            return Task.FromResult(0);
        }
    }
}
