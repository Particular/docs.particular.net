using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler : IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public void Handle(IMyEvent message)
    {
        log.Info($"Subscriber 2 received IEvent with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
    }
}