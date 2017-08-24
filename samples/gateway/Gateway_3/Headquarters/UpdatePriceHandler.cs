using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region UpdatePriceHandler

public class UpdatePriceHandler :
    IHandleMessages<UpdatePrice>
{
    static ILog log = LogManager.GetLogger<UpdatePriceHandler>();

    public Task Handle(UpdatePrice message, IMessageHandlerContext context)
    {
        log.Info("Price update received from the webclient, going to push to RemoteSite");
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
        return context.SendToSites(siteKeys, priceUpdated);
    }
}

#endregion