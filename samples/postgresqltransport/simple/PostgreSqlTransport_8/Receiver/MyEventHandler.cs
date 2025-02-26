using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class MyEventHandler : IHandleMessages<MyEvent>
{
    private static readonly ILogger<MyCommandHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<MyCommandHandler>();

    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        logger.LogInformation($"Hello from {nameof(MyEventHandler)}");
        return Task.CompletedTask;
    }
}