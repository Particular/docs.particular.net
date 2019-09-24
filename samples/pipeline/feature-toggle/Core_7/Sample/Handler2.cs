using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 :
    IHandleMessages<Message>
{
    static ILog log = LogManager.GetLogger<Handler2>();

    public Task Handle(Message message, IMessageHandlerContext context)
    {
        log.Info("Handler2 received message");
        return Task.CompletedTask;
    }
}
