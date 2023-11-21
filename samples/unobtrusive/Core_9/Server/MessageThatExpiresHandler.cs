using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MessageThatExpiresHandler :
    IHandleMessages<MessageThatExpires>
{
    static ILog log = LogManager.GetLogger<MessageThatExpiresHandler>();

    public Task Handle(MessageThatExpires message, IMessageHandlerContext context)
    {
        log.Info($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
        return Task.CompletedTask;
    }
}