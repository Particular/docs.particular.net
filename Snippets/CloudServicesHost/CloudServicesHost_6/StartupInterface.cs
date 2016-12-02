using NServiceBus;

#region CloudServicesStartAndStop
public class Bootstrapper :
    IWantToRunWhenBusStartsAndStops
{

    public void Start()
    {
        // Do startup actions here.
    }

    public void Stop()
    {
        // Do cleanup actions here.
    }
}
#endregion
