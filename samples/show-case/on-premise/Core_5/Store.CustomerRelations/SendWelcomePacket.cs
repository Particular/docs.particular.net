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
        log.Info($"Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {message.ClientId}");
    }
}