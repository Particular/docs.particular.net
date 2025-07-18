using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class Message2Handler(ILogger<Message2Handler> logger) :
    IHandleMessages<Message2>
{
    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received Message2: {Property}", message.Property);
        return Task.CompletedTask;
    }
}