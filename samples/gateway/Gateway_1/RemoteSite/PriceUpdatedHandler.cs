using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler :
    IHandleMessages<PriceUpdated>
{
    static ILog log = LogManager.GetLogger(typeof(PriceUpdatedHandler));
    IBus bus;

    public PriceUpdatedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(PriceUpdated message)
    {
        var messageHeader = bus.GetMessageHeader(message, Headers.OriginatingSite);
        log.Info($"Price update for product: {message.ProductId} received. Going to reply over channel: {messageHeader}");

        var updateAcknowledged = new PriceUpdateAcknowledged
        {
            BranchOffice = "RemoteSite"
        };
        bus.Reply(updateAcknowledged);
    }
}

#endregion