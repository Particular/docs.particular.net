using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

#region UpdatePriceHandler
public class UpdatePriceHandler : IHandleMessages<UpdatePrice>
{
    public async Task Handle(UpdatePrice message, IMessageHandlerContext context)
    {
        Console.WriteLine("Price update request received from the webclient, going to push it to RemoteSite");
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