using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class AuditThisMessageHandler(ILogger<AuditThisMessageHandler> logger) :
    IHandleMessages<AuditThisMessage>
{
    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling {MessageType}", message.GetType().Name);
        return Task.CompletedTask;
    }
}