using Microsoft.AspNetCore.SignalR;

namespace ClientHub;

#region StockTickHub

public class StockTicksHub : Hub<IEmitStockTicks>;

public interface IEmitStockTicks
{
    Task PushStockTick(StockTick tick);
}
#endregion