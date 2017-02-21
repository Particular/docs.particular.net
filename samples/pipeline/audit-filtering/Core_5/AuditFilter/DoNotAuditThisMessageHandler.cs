using NServiceBus;
using NServiceBus.Logging;

public class DoNotAuditThisMessageHandler :
    IHandleMessages<DoNotAuditThisMessage>
{
    static ILog log = LogManager.GetLogger<DoNotAuditThisMessageHandler>();

    public void Handle(DoNotAuditThisMessage message)
    {
        log.Info($"Handling {message.GetType().Name}");
    }
}