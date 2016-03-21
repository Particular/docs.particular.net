using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region UpdatePriceHandler
public class UpdatePriceHandler : IHandleMessages<UpdatePrice>
{
    static ILog log = LogManager.GetLogger<UpdatePriceHandler>();

    public async Task Handle(UpdatePrice message, IMessageHandlerContext context)
    {
        log.Info("Price update request received from the webclient, going to push it to RemoteSite");
        string[] siteKeys = {
                                "RemoteSite",
                            };
        PriceUpdated priceUpdated = new PriceUpdated
                                    {
                                        ProductId = message.ProductId,
                                        NewPrice = message.NewPrice,
                                        ValidFrom = message.ValidFrom,
                                    };
        await context.SendToSites(siteKeys, priceUpdated);
    }
}
#endregion