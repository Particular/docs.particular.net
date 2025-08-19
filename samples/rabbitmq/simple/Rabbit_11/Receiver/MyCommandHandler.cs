namespace Receiver;

using Microsoft.Extensions.Logging;
using Shared;

public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from {HandlerType}", nameof(MyCommandHandler));

        return Task.CompletedTask;
    }
}