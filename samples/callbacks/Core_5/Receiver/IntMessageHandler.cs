using NServiceBus;
using NServiceBus.Logging;

#region IntMessageHandler

public class IntMessageHandler :
    IHandleMessages<IntMessage>
{
    static ILog log = LogManager.GetLogger<IntMessageHandler>();
    IBus bus;

    public IntMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(IntMessage message)
    {
        log.Info("Message received, Returning");
        bus.Return(10);
    }
}

#endregion