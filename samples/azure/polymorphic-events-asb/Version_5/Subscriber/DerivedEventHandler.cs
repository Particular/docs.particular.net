using Events;
using NServiceBus;
using NServiceBus.Logging;

public class DerivedEventHandler : IHandleMessages<DerivedEvent>
{
    static ILog logger = LogManager.GetLogger<DerivedEventHandler>();

    public void Handle(DerivedEvent message)
    {
        logger.InfoFormat("Received DerivedEvent. EventId: {0} Data: {1}", message.EventId, message.Data);
    }
}