using Microsoft.Extensions.Logging;

public class DoNotAuditThisMessageHandler(ILogger<DoNotAuditThisMessageHandler> logger) : IHandleMessages<DoNotAuditThisMessage>
{
    public Task Handle(DoNotAuditThisMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling {MessageType}", message.GetType().Name);
        return Task.CompletedTask;
    }
}