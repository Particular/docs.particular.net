using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class SendLimitedTimeOffer :
    IHandleMessages<ClientBecamePreferred>
{
    static ILog log = LogManager.GetLogger<SendLimitedTimeOffer>();

    public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        log.Info($"Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {message.ClientId}");
        return Task.CompletedTask;
    }
}