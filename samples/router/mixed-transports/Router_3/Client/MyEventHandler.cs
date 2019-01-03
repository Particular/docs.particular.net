using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyEventHandler :
    IHandleMessages<MyEvent>
{
    static ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        log.Info($"Event {message.Id}");
        return Task.CompletedTask;
    }
}