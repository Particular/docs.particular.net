using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class DoNotAuditThisMessageHandler :
    IHandleMessages<DoNotAuditThisMessage>
{
    static ILog log = LogManager.GetLogger<DoNotAuditThisMessageHandler>();

    public Task Handle(DoNotAuditThisMessage message, IMessageHandlerContext context)
    {
        log.Info($"Handling {message.GetType().Name}");
        return Task.FromResult(0);
    }
}