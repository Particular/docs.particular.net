using Microsoft.AspNet.SignalR;

public class StockTicksHub : Hub<IEmitStockTicks> { }

public interface IEmitStockTicks
{
    void StockTick(StockTick tick);
}