using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    static ILog log = LogManager.GetLogger<AnotherEventMessageHandler>();

    public Task Handle(AnotherEventMessage message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Subscriber 1 received AnotherEventMessage with Id {message.EventId}.");
        log.InfoFormat($"Message time: {message.Time}.");
        log.InfoFormat($"Message duration: {message.Duration}.");
        return Task.FromResult(0);
    }
}