using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Events;

class SendWelcomePacket(ILogger<SendWelcomePacket> logger) :
    IHandleMessages<ClientBecamePreferred>
{
    public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        logger.LogInformation("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {ClientId}", message.ClientId);
        return Task.CompletedTask;
    }
}