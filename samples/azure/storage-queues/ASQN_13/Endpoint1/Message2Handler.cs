using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message2Handler(ILogger<Message2Handler> logger) :
    IHandleMessages<Message2>
{
    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}
