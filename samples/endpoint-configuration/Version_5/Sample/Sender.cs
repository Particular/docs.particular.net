using NServiceBus;

class Sender : IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public Sender(IBus bus)
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