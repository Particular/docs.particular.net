using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from {Handler}", nameof(MyCommandHandler));
        return Task.CompletedTask;
    }
}