using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler : 
    IHandleMessages<EnumMessage>,
    IHandleMessages<IntMessage>,
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public async Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received EnumMessage. Property:'{message.Property}'");
        await context.Reply(Status.Ok);
    }

    public async Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received IntMessage. Property:'{message.Property}'");
        await context.Reply(10);
    }

    public async Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received ObjectMessage. Property:'{message.Property}'");
        await context.Reply(new ReplyMessage
        {
            Property = $"Handler Received '{message.Property}'"
        });
    }
}