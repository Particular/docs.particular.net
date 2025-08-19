using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace AsbEndpoint;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received MyEvent: {Property}", message.Property);
        var otherEvent = new OtherEvent
        {
            Property = "event from ASB endpoint"
        };
        logger.LogInformation("Publishing OtherEvent: {Property}", otherEvent.Property);
        return context.Publish(otherEvent);
    }
}