using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithXml>
{
    static ILog log = LogManager.GetLogger<MessageHandler>();

    public Task Handle(MessageWithJson message, IMessageHandlerContext context)
    {
        log.Info($"Received JSON message with property '{message.SomeProperty}'");
        return Task.CompletedTask;
    }

    public Task Handle(MessageWithXml message, IMessageHandlerContext context)
    {
        log.Info($"Received XML message with property '{message.SomeProperty}'");
        return Task.CompletedTask;
    }
}