using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation($"Hello from {nameof(MyCommandHandler)}");
        return Task.CompletedTask;
    }
}