using NServiceBus;

#region RunWhenStartsAndStops
public class MessageSender : IWantToRunAtStartup
{
    IBus bus;

    public MessageSender(IBus bus)
    {
        this.bus = bus;
    }

    public void Run()
    {
        bus.SendLocal(new MyMessage());
    }

    public void Stop()
    {
    }
}
#endregion