using NServiceBus;

public class WantToRunWhenBusStartsAndStops :
    IWantToRunWhenBusStartsAndStops
{
    public void Start()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Start");
    }

    public void Stop()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Stop");
    }
}