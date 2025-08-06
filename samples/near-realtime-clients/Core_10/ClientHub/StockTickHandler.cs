using Microsoft.AspNetCore.SignalR;
namespace ClientHub;


#region StockTickHandler

public class StockTickHandler(IHubContext<StockTicksHub> hub, ILogger<StockTickHandler> logger)
    : IHandleMessages<StockTick>
{
    public Task Handle(StockTick message, IMessageHandlerContext context)
    {
        logger.LogInformation($"StockTick event received for Symbol {message.Symbol} with timestamp: {message.Timestamp:O}. Press any key to exit.");
        return hub.Clients.All.SendAsync("PushStockTick", message, context.CancellationToken);
    }
}

#endregion