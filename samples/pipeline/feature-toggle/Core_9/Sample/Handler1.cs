using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler1 :
    IHandleMessages<Message>
{
    static ILog log = LogManager.GetLogger<Handler1>();

    public Task Handle(Message message, IMessageHandlerContext context)
    {
        log.Info("Handler1 received message");
        return Task.CompletedTask;
    }

}