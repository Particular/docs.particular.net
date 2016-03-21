using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler : IHandleMessages<EventMessage>
{
    static ILog log = LogManager.GetLogger<EventMessageHandler>();

    public void Handle(EventMessage message)
    {
        log.InfoFormat("Subscriber 1 received EventMessage with Id {0}.", message.EventId);
        log.InfoFormat("Message time: {0}.", message.Time);
        log.InfoFormat("Message duration: {0}.", message.Duration);
    }
}