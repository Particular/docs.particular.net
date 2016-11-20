using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler :
    IHandleMessages<EnumMessage>,
    IHandleMessages<IntMessage>,
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received EnumMessage. Property:'{message.Property}'");
        return context.Reply(Status.Ok);
    }

    public Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received IntMessage. Property:'{message.Property}'");
        return context.Reply(10);
    }

    public Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received ObjectMessage. Property:'{message.Property}'");
        var replyMessage = new ReplyMessage
        {
            Property = $"Handler Received '{message.Property}'"
        };
        return context.Reply(replyMessage);
    }
}