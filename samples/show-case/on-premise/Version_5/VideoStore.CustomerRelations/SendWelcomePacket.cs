namespace VideoStore.CustomerRelations
{
    using System;
    using System.Diagnostics;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class SendWelcomePacket : IHandleMessages<ClientBecamePreferred>
    {
        public IBus Bus { get; set; }

        public void Handle(ClientBecamePreferred message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }
            Console.WriteLine("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {0}", message.ClientId);
        }
    }
}
