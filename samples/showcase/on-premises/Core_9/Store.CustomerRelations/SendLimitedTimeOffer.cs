using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Events;

class SendLimitedTimeOffer(ILogger<SendLimitedTimeOffer> logger) :
    IHandleMessages<ClientBecamePreferred>
{
      public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        logger.LogInformation("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {ClientId}", message.ClientId);
        return Task.CompletedTask;
    }
}