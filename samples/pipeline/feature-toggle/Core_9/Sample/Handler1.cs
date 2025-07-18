using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Handler1(ILogger<Handler1> logger) : IHandleMessages<Message>
{
    public Task Handle(Message message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handler1 received message");
        return Task.CompletedTask;
    }
}