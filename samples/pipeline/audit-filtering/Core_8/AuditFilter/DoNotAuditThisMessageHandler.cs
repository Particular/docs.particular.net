using NServiceBus.Logging;

public class DoNotAuditThisMessageHandler : IHandleMessages<DoNotAuditThisMessage>
{
    static readonly ILog log = LogManager.GetLogger<DoNotAuditThisMessageHandler>();

    public Task Handle(DoNotAuditThisMessage message, IMessageHandlerContext context)
    {
        log.Info($"Handling {message.GetType().Name}");
        return Task.CompletedTask;
    }
}