using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MessageToIncludeAudit>,
    IHandleMessages<MessageToExcludeFromAudit>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MessageToIncludeAudit message, IMessageHandlerContext context)
    {
        log.Info("MessageToIncludeAudit received");
        return Task.CompletedTask;
    }

    public Task Handle(MessageToExcludeFromAudit message, IMessageHandlerContext context)
    {
        log.Info("MessageToExcludeFromAudit received");
        return Task.CompletedTask;
    }
}