using NServiceBus;
using NServiceBus.Logging;

#region ObjectMessageHandler

public class ObjectMessageHandler :
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<ObjectMessageHandler>();
    IBus bus;

    public ObjectMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(ObjectMessage message)
    {
        log.Info("Message received, Returning");
        bus.Reply(new ObjectResponseMessage
        {
            Property = "PropertyValue"
        });
    }
}

#endregion