
using System;
using NServiceBus;
using Shared;

#region UpdatePriceHandler
public class UpdatePriceHandler : IHandleMessages<UpdatePrice>
{
    IBus bus;

    public UpdatePriceHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(UpdatePrice message)
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
        bus.SendToSites(siteKeys, priceUpdated);
    }
}
#endregion