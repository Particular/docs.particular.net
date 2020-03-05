using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler : IHandleMessages<MyEvent>
{
    static ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received MyEvent: {message.Property}");
        return context.Publish<OtherEvent>(@event => { @event.Property = message.Property; });
    }
}