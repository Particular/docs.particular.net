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
        var header = bus.GetMessageHeader(message, Headers.OriginatingSite);
        log.Info($"Price update for: {message.ProductId} received. Reply over channel: {header}");

        var updateAcknowledged = new PriceUpdateAcknowledged
        {
            BranchOffice = "RemoteSite"
        };
        bus.Reply(updateAcknowledged);
    }
}

#endregion