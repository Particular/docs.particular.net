using System.Threading.Tasks;
using NServiceBus;

public class WantToRunWhenBusStartsAndStops :
    IWantToRunWhenBusStartsAndStops
{
    public Task StartAsync()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Start");
        return Task.FromResult(0);
    }

    public Task StopAsync()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Stop");
        return Task.FromResult(0);
    }
}