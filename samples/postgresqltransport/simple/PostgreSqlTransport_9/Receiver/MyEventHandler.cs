using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{

    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from {Handler}", nameof(MyEventHandler));
        return Task.CompletedTask;
    }
}