using NServiceBus;
using NServiceBus.Logging;

#region EnumMessageHandler
public class EnumMessageHandler :
    IHandleMessages<EnumMessage>
{
    static ILog log = LogManager.GetLogger(typeof(EnumMessageHandler));
    IBus bus;

    public EnumMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(EnumMessage message)
    {
        log.Info("Message received, Returning");
        bus.Return(Status.OK);
    }
}


#endregion