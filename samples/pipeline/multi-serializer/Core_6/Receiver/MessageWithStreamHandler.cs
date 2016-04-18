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
        log.InfoFormat("Received JSON message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }

    public Task Handle(MessageWithXml message, IMessageHandlerContext context)
    {
        log.InfoFormat("Received XML message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }
    
}