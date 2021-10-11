using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler :
    IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        log.Info($"Received MyCommand: {message.Property}");
        return context.Publish<MyEvent>(@event => { @event.Property = "event from MSMQ endpoint"; });
    }
}