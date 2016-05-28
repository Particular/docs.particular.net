using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler : IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public Task Handle(IMyEvent message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Subscriber 2 received IEvent with Id {message.EventId}.");
        log.InfoFormat($"Message time: {message.Time}.");
        log.InfoFormat($"Message duration: {message.Duration}.");
        return Task.FromResult(0);
    }

}