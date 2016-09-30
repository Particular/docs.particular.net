using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler :
    IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public Task Handle(IMyEvent message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber 2 received IEvent with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
        return Task.CompletedTask;
    }

}