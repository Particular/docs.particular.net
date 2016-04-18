using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class SendLimitedTimeOffer : IHandleMessages<ClientBecamePreferred>
{
    static ILog log = LogManager.GetLogger<SendLimitedTimeOffer>();

    public void Handle(ClientBecamePreferred message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        log.InfoFormat("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {0}", message.ClientId);
    }
}