using Events;
using NServiceBus;
using NServiceBus.Logging;

public class BaseEventHandler : IHandleMessages<BaseEvent>
{
    static ILog logger = LogManager.GetLogger<BaseEventHandler>();

    public void Handle(BaseEvent message)
    {
        logger.InfoFormat("Received BaseEvent. EventId: {0}", message.EventId);
    }
}