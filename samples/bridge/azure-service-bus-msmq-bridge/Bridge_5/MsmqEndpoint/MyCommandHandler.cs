using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace MsmqEndpoint;

public class MyCommandHandler :
    IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        log.Info($"Received MyCommand: {message.Property}");
        var myEvent = new MyEvent
        {
            Property = "event from MSMQ endpoint"
        };
        log.Info($"Publishing MyEvent: {myEvent.Property}");
        return context.Publish(myEvent);
    }
}