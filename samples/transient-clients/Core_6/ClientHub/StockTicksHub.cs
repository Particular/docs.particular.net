using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

public class StockTicksHub : Hub<IEmitStockTicks> { }

public interface IEmitStockTicks
{
    Task ForwardStockTick(StockTick tick);
}
