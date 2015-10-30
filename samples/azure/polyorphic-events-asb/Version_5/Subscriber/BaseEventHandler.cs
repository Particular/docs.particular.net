using Events;
using NServiceBus;
using NServiceBus.Logging;

public class BaseEventHandler : IHandleMessages<BaseEvent>
{
    IBus bus;
    static ILog logger = LogManager.GetLogger<BaseEventHandler>();

    public BaseEventHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(BaseEvent message)
    {
        logger.InfoFormat("Received BaseEvent. EventId: {0}", message.EventId);
    }
}