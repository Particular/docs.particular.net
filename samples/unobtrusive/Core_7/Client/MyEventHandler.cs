using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler :
    IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(IMyEvent message, IMessageHandlerContext context)
    {
        log.Info($"IMyEvent received from server with id:{message.EventId}");
        return Task.CompletedTask;
    }

}