using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received {nameof(MyEvent)} with a payload of {eventMessage.Data?.Length ?? 0} bytes.");
        return Task.CompletedTask;
    }
}