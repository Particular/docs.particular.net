using NServiceBus;

#region RunWhenStartsAndStops
public class MessageSender : IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public MessageSender(IBus bus)
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
#endregion