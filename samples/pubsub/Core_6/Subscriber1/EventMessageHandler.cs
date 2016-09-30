using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler :
    IHandleMessages<EventMessage>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public Task Handle(EventMessage message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber 1 received EventMessage with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
        return Task.CompletedTask;
    }
}