using NServiceBus;
using NServiceBus.Logging;

public class EventMessageHandler : IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger(typeof(EventMessageHandler));

    public void Handle(IMyEvent message)
    {
        log.InfoFormat("Subscriber 2 received IEvent with Id {0}.", message.EventId);
        log.InfoFormat("Message time: {0}.", message.Time);
        log.InfoFormat("Message duration: {0}.", message.Duration);
    }
}