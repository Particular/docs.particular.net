using NServiceBus;

public class WantToRunWhenBusStartsAndStops :
    IWantToRunWhenBusStartsAndStops
{

    void IWantToRunWhenBusStartsAndStops.Start()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Start");
    }


    void IWantToRunWhenBusStartsAndStops.Stop()
    {
        Logger.WriteLine("Inside IWantToRunWhenBusStartsAndStops.Stop");
    }

}