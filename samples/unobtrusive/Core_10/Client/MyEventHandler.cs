using Events;
using Microsoft.Extensions.Logging;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("MyEvent received from server with id:{EventId}", message.EventId);
        return Task.CompletedTask;
    }
}