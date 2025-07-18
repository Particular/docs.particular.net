using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Handler2(ILogger<Handler2> logger) : IHandleMessages<Message>
{
    public Task Handle(Message message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handler2 received message");
        return Task.CompletedTask;
    }
}
