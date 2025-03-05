using Microsoft.Extensions.Logging;
using Shared;

#region UpdatePriceHandler
public class UpdatePriceHandler : IHandleMessages<UpdatePrice>
{
    private static readonly ILogger<UpdatePriceHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<UpdatePriceHandler>();


    public Task Handle(UpdatePrice message, IMessageHandlerContext context)
    {
        logger.LogInformation("Price update received from the webclient, going to push to RemoteSite");
        string[] siteKeys =
        [
            "RemoteSite"
        ];
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