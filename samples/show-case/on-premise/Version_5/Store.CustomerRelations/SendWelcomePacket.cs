using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class SendWelcomePacket : IHandleMessages<ClientBecamePreferred>
{
    static ILog log = LogManager.GetLogger<SendWelcomePacket>();

    public void Handle(ClientBecamePreferred message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        log.InfoFormat("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {0}", message.ClientId);
    }
}