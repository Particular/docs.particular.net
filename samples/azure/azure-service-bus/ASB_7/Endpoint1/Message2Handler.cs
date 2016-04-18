using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Message2Handler : IHandleMessages<Message2>
{
    static ILog logger = LogManager.GetLogger<Message2Handler>();

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received Message2: {0}", message.Property);
        return Task.FromResult(0);
    }
}