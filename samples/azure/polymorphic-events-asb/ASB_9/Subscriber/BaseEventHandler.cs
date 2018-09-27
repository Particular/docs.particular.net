using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class BaseEventHandler :
    IHandleMessages<BaseEvent>
{
    static ILog log = LogManager.GetLogger<BaseEventHandler>();

    public Task Handle(BaseEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received BaseEvent. EventId: {message.EventId}");
        return Task.CompletedTask;
    }
}