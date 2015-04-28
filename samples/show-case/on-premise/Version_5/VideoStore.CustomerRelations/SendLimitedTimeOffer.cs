namespace VideoStore.CustomerRelations
{
    using System;
    using System.Diagnostics;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class SendLimitedTimeOffer : IHandleMessages<ClientBecamePreferred>
    {
        public void Handle(ClientBecamePreferred message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }
            Console.WriteLine("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {0}", message.ClientId);
        }
    }
}
