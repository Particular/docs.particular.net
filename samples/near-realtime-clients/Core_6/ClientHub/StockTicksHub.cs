using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

#region StockTickHub

public class StockTicksHub :
    Hub<IEmitStockTicks>
{
}

public interface IEmitStockTicks
{
    Task PushStockTick(StockTick tick);
}
#endregion