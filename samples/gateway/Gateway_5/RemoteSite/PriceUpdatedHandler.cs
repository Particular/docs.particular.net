using Microsoft.Extensions.Logging;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler : IHandleMessages<PriceUpdated>
{
    private static readonly ILogger<PriceUpdatedHandler> logger =
     LoggerFactory.Create(builder =>
     {
         builder.AddConsole();
     }).CreateLogger<PriceUpdatedHandler>();


    public Task Handle(PriceUpdated message, IMessageHandlerContext context)
    {
        var header = context.MessageHeaders[Headers.OriginatingSite];
        logger.LogInformation($"Price update for: {message.ProductId} received. Reply over channel: {header}");

        var updateAcknowledged = new PriceUpdateAcknowledged
        {
            BranchOffice = "RemoteSite"
        };
        return context.Reply(updateAcknowledged);
    }
}

#endregion