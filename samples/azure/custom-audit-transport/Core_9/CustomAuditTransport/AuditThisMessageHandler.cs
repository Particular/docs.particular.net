using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class AuditThisMessageHandler(ILogger<AuditThisMessageHandler> logger) :
    IHandleMessages<AuditThisMessage>
{
    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        if(message.Error) throw new System.Exception("Simulated error in message handler");

        logger.LogInformation("Handling {MessageType} with content: {Content}", message.GetType().Name, message.Content);
        return Task.CompletedTask;
    }
}