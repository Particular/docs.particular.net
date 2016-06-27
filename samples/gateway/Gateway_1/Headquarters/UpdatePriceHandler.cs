using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region UpdatePriceHandler
public class UpdatePriceHandler : IHandleMessages<UpdatePrice>
{
    static ILog log = LogManager.GetLogger<UpdatePriceHandler>();
    IBus bus;

    public UpdatePriceHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(UpdatePrice message)
    {
        log.Info("Price update request received from the webclient, going to push it to RemoteSite");
        string[] siteKeys =
        {
            "RemoteSite"
        };
        var priceUpdated = new PriceUpdated
        {
            ProductId = message.ProductId,
            NewPrice = message.NewPrice,
            ValidFrom = message.ValidFrom,
        };
        bus.SendToSites(siteKeys, priceUpdated);
    }
}
#endregion