using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus;

public class Message2Handler :
    IHandleMessages<Message2>
{
    static ILog logger = LogManager.GetLogger<Message2Handler>();

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        logger.Info($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}