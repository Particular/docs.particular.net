using log4net;
using NServiceBus;

#region ObjectMessageHandler
public class ObjectMessageHandler : IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger(typeof(ObjectMessageHandler));
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