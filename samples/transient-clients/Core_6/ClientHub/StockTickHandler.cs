using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NServiceBus;

public class StockTickHandler : IHandleMessages<StockTick>
{
    private readonly IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<StockTicksHub>();

    public Task Handle(StockTick message, IMessageHandlerContext context)
    {
        Console.WriteLine($"StockTick event received for Symbol {message.Symbol} with timestamp: {message.Timestamp:O}. Press any key to exit.");
        return hub.Clients.All.StockTick(message);
    }


}
