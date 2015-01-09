using NServiceBus;

public class Startup : IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public Startup(IBus bus)
    {
        this.bus = bus;
    }

    public void Start()
    {
        bus.SendLocal(new MyMessage());
    }

    public void Stop()
    {
    }
}