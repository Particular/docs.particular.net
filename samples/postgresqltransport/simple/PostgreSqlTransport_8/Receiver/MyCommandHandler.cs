using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    private static readonly ILogger<MyCommandHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<MyCommandHandler>();

    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation($"Hello from {nameof(MyCommandHandler)}");
        return Task.CompletedTask;
    }
}