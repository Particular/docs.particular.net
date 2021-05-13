using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingFinishedHandler :
    IHandleMessages<LongProcessingFinished>
{
    static ILog log = LogManager.GetLogger<LongProcessingFinishedHandler>();

    public Task Handle(LongProcessingFinished message, IMessageHandlerContext context)
    {
        log.Info($"Request with ID {message.Id} was successfully finished.");
        return Task.CompletedTask;
    }
}