using NServiceBus;
using NServiceBus.Unicast;

public class Startup : IWantToRunWhenTheBusStarts
{
    IBus bus;

    public Startup(IBus bus)
    {
        this.bus = bus;
    }

    public void Run()
    {
        bus.SendLocal(new MyMessage());
    }

}