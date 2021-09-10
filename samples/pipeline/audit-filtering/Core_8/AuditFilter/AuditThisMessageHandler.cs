using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class AuditThisMessageHandler :
    IHandleMessages<AuditThisMessage>
{
    static ILog log = LogManager.GetLogger<AuditThisMessageHandler>();

    public Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        log.Info($"Handling {message.GetType().Name}");
        return Task.CompletedTask;
    }
}