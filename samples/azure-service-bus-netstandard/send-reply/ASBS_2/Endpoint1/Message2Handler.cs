using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Message2Handler :
    IHandleMessages<Message2>
{
    static ILog log = LogManager.GetLogger<Message2Handler>();

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        log.Info($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}