using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class SendWelcomePacket : IHandleMessages<ClientBecamePreferred>
{
    static ILog log = LogManager.GetLogger<SendWelcomePacket>();

    public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        log.InfoFormat("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {0}", message.ClientId);
        return Task.FromResult(0);
    }

}