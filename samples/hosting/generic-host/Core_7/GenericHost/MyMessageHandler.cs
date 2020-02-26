using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message #{message.Number}");
        return Task.CompletedTask;
    }

    private readonly ILogger<MyMessageHandler> logger;
}