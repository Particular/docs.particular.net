using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class AuditThisMessageHandler : IHandleMessages<AuditThisMessage>
{
    static ILog logger = LogManager.GetLogger<DoNotAuditThisMessageHandler>();

    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        logger.Info($"Handling {message.GetType().Name}");
        return Task.FromResult(0);
    }
}