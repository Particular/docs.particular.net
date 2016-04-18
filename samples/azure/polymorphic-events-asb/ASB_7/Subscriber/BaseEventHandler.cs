using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class BaseEventHandler : IHandleMessages<BaseEvent>
{
    static ILog logger = LogManager.GetLogger<BaseEventHandler>();

    public Task Handle(BaseEvent message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received BaseEvent. EventId: {0}", message.EventId);
        return Task.FromResult(0);
    }
}