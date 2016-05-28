using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    static ILog log = LogManager.GetLogger<AnotherEventMessageHandler>();

    public Task Handle(AnotherEventMessage message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber 1 received AnotherEventMessage with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
        return Task.FromResult(0);
    }
}