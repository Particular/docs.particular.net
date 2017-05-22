using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using NServiceBus.Logging;

#region StockTickHandler

public class StockTickHandler :
    IHandleMessages<StockTick>
{
    static ILog log = LogManager.GetLogger<StockTickHandler>();
    IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<StockTicksHub>();

    public Task Handle(StockTick message, IMessageHandlerContext context)
    {
        log.Info($"StockTick event received for Symbol {message.Symbol} with timestamp: {message.Timestamp:O}. Press any key to exit.");
        return hub.Clients.All.PushStockTick(message);
    }
}

#endregion