using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingRequestReplyHandler : IHandleMessages<LongProcessingRequestReply>
{
    static ILog log = LogManager.GetLogger<LongProcessingRequestReplyHandler>();

    public Task Handle(LongProcessingRequestReply message, IMessageHandlerContext context)
    {
        log.Info($"Request ID {message.Id} was enqueued for processing.");

        return Task.FromResult(0);
    }
}