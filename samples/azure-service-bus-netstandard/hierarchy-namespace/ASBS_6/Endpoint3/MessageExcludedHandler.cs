using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MessageExcludedHandler (ILogger<MessageExcludedHandler> logger):
    IHandleMessages<MessageExcluded>
{

    public Task Handle(MessageExcluded message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received MessageExcluded: {Property}", message.Property);

        return Task.CompletedTask;
    }
}