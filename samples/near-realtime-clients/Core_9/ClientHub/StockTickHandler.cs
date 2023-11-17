using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace ClientHub;


#region StockTickHandler

public class StockTickHandler :
    IHandleMessages<StockTick>
{
    static readonly ILog log = LogManager.GetLogger<StockTickHandler>();
    readonly IHubContext<StockTicksHub> hub;

    public StockTickHandler(IHubContext<StockTicksHub> hub)
    {
        this.hub = hub;
    }

    public Task Handle(StockTick message, IMessageHandlerContext context)
    {
        log.Info($"StockTick event received for Symbol {message.Symbol} with timestamp: {message.Timestamp:O}. Press any key to exit.");
        return hub.Clients.All.SendAsync("PushStockTick", message, context.CancellationToken);
    }
}

#endregion