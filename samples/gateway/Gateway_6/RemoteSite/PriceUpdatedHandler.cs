using Microsoft.Extensions.Logging;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler(ILogger<PriceUpdatedHandler> logger) : IHandleMessages<PriceUpdated>
{
    public Task Handle(PriceUpdated message, IMessageHandlerContext context)
    {
        var header = context.MessageHeaders[Headers.OriginatingSite];
        logger.LogInformation("Price update for: {ProductId} received. Reply over channel: {Channel}", message.ProductId, header);

        var updateAcknowledged = new PriceUpdateAcknowledged
        {
            BranchOffice = "RemoteSite"
        };
        return context.Reply(updateAcknowledged);
    }
}

#endregion