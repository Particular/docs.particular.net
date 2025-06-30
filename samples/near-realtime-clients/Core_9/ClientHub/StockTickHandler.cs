using Microsoft.AspNetCore.SignalR;
namespace ClientHub;


#region StockTickHandler

public class StockTickHandler :
    IHandleMessages<StockTick>
{
    readonly IHubContext<StockTicksHub> hub;
    private readonly ILogger<StockTickHandler> logger;

    public StockTickHandler(IHubContext<StockTicksHub> hub, ILogger<StockTickHandler> logger)
    {
        this.hub = hub;
        this.logger = logger;
    }

    public Task Handle(StockTick message, IMessageHandlerContext context)
    {
        logger.LogInformation("StockTick event received for Symbol {Symbol} with timestamp: {Timestamp}. Press any key to exit.", message.Symbol, message.Timestamp);
        return hub.Clients.All.SendAsync("PushStockTick", message, context.CancellationToken);
    }
}

#endregion