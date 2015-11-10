using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;

class SendWelcomePacket : IHandleMessages<ClientBecamePreferred>
{

    public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        Console.WriteLine("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {0}", message.ClientId);
        return Task.FromResult(0);
    }

}