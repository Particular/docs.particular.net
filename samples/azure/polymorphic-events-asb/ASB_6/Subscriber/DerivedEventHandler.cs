using Events;
using NServiceBus;
using NServiceBus.Logging;

public class DerivedEventHandler : IHandleMessages<DerivedEvent>
{
    static ILog log = LogManager.GetLogger<DerivedEventHandler>();

    public void Handle(DerivedEvent message)
    {
        log.Info($"Received DerivedEvent. EventId: {message.EventId} Data: {message.Data}");
    }
}