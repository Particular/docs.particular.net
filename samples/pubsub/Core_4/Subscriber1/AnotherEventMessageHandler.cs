using NServiceBus;
using NServiceBus.Logging;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    static ILog log = LogManager.GetLogger(typeof(AnotherEventMessageHandler));

    public void Handle(AnotherEventMessage message)
    {
        log.Info($"Subscriber 1 received AnotherEventMessage with Id {message.EventId}.");
        log.Info($"Message time: {message.Time}.");
        log.Info($"Message duration: {message.Duration}.");
    }
}