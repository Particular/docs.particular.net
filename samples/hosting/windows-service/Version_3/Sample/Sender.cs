using NServiceBus;
using NServiceBus.Unicast;

class Sender : IWantToRunWhenTheBusStarts
{
    IBus bus;

    public Sender(IBus bus)
    {
        this.bus = bus;
    }

    public void Run()
    {
        bus.SendLocal(new MyMessage());
    }
}