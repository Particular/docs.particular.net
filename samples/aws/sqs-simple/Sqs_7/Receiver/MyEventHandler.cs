using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Received {MessageType} with a payload of {PayloadLength} bytes.", nameof(MyEvent), eventMessage.Data?.Length ?? 0);
        return Task.CompletedTask;
    }
}