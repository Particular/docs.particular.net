using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler : IHandleMessages<EventMessage>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public Task Handle(EventMessage message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Subscriber 1 received EventMessage with Id {message.EventId}.");
        log.InfoFormat($"Message time: {message.Time}.");
        log.InfoFormat($"Message duration: {message.Duration}.");
        return Task.FromResult(0);
    }
}