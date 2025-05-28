using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class AuditThisMessageHandler(ILogger<AuditThisMessageHandler> logger) :
    IHandleMessages<AuditThisMessage>
{
    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Handling {message.GetType().Name} with content: {message.Content}");
        return Task.CompletedTask;
    }
}