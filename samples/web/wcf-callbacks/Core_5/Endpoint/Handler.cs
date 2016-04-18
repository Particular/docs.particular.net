using NServiceBus;
using NServiceBus.Logging;

public class Handler : 
    IHandleMessages<EnumMessage>,
    IHandleMessages<IntMessage>,
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<Handler>();
    IBus bus;

    public Handler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(EnumMessage message)
    {
        log.InfoFormat("Received EnumMessage. Property:'{0}'", message.Property);
        bus.Return(Status.Ok);
    }

    public void Handle(IntMessage message)
    {
        log.InfoFormat("Received IntMessage. Property:'{0}'", message.Property);
        bus.Return(10);
    }

    public void Handle(ObjectMessage message)
    {
        log.InfoFormat("Received ObjectMessage. Property:'{0}'", message.Property);
        bus.Reply(new ReplyMessage
        {
            Property = string.Format("Handler Received '{0}'", message.Property)
        });
    }
}