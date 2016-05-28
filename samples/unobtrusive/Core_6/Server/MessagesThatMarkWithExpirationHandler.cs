using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MessagesThatMarkWithExpirationHandler : IHandleMessages<MessageThatExpires>
{
    static ILog log = LogManager.GetLogger<MessagesThatMarkWithExpirationHandler>();

    public Task Handle(MessageThatExpires message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
        return Task.FromResult(0);
    }
}