using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Hello from Instance 2");
        return Task.CompletedTask;
    }
}