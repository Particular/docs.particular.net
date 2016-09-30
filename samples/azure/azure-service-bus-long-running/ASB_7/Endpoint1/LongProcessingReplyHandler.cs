using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingReplyHandler :
    IHandleMessages<LongProcessingReply>
{
    static ILog log = LogManager.GetLogger<LongProcessingReplyHandler>();

    public Task Handle(LongProcessingReply message, IMessageHandlerContext context)
    {
        log.Info($"Request ID {message.Id} was enqueued for processing.");
        return Task.CompletedTask;
    }
}