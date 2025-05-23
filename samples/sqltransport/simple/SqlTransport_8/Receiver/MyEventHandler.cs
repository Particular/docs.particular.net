using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

sealed class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from {HandlerName}", nameof(MyEventHandler));
        return Task.CompletedTask;
    }
}