using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler :
    IHandleMessages<MyEvent>
{
    static ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        log.Info($"MyEvent received from server with id:{message.EventId}");
        return Task.CompletedTask;
    }
}