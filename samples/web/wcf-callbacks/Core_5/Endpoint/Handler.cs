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
        log.Info($"Received EnumMessage. Property:'{message.Property}'");
        bus.Return(Status.Ok);
    }

    public void Handle(IntMessage message)
    {
        log.Info($"Received IntMessage. Property:'{message.Property}'");
        bus.Return(10);
    }

    public void Handle(ObjectMessage message)
    {
        log.Info($"Received ObjectMessage. Property:'{message.Property}'");
        var replyMessage = new ReplyMessage
        {
            Property = $"Handler Received '{message.Property}'"
        };
        bus.Reply(replyMessage);
    }
}