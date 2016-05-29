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
        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }

    public void Stop()
    {
    }
}
#endregion