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
        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }

    public void Stop()
    {
    }
}