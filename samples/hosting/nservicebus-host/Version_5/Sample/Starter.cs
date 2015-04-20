using NServiceBus;

public class Starter :IWantToRunWhenBusStartsAndStops{
    IBus bus;

    public Starter(IBus bus)
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