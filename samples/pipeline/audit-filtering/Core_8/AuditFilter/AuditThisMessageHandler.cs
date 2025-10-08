using NServiceBus.Logging;

public class AuditThisMessageHandler : IHandleMessages<AuditThisMessage>
{
    static readonly ILog log = LogManager.GetLogger<AuditThisMessageHandler>();

    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        log.Info($"Handling {message.GetType().Name}");
        return Task.CompletedTask;
    }
}