using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class DoNotAuditThisMessageHandler : IHandleMessages<DoNotAuditThisMessage>
{
    static ILog logger = LogManager.GetLogger<DoNotAuditThisMessageHandler>();

    public Task Handle(DoNotAuditThisMessage message, IMessageHandlerContext context)
    {
        logger.Info($"Handling {message.GetType().Name}");
        return Task.FromResult(0);
    }
}