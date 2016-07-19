using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler :
    IHandleMessages<EventMessage>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public void Handle(EventMessage message)
    {
        log.Info($"Subscriber 1 received EventMessage with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
    }
}