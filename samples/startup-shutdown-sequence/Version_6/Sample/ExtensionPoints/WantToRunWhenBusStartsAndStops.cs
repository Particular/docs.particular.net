using System.Threading.Tasks;
using NServiceBus;

public class WantToRunWhenBusStartsAndStops :
    IWantToRunWhenBusStartsAndStops
{
    public Task Start(IBusContext context)
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Start");
        return Task.FromResult(0);
    }

    public Task Stop(IBusContext context)
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Stop");
        return Task.FromResult(0);
    }
}