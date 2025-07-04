namespace Receiver;

using Microsoft.Extensions.Logging;
using Shared;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from {HandlerType}", nameof(MyEventHandler));

        return Task.CompletedTask;
    }
}