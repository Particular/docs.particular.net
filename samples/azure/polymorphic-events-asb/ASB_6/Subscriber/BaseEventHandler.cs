using Events;
using NServiceBus;
using NServiceBus.Logging;

public class BaseEventHandler :
    IHandleMessages<BaseEvent>
{
    static ILog log = LogManager.GetLogger<BaseEventHandler>();

    public void Handle(BaseEvent message)
    {
        log.Info($"Received BaseEvent. EventId: {message.EventId}");
    }
}