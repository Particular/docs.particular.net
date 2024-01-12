using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    static readonly ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        context.MessageHeaders.TryGetValue("tenant_id", out var tenantId);

        log.Info($"Tenant {tenantId} order {message.OrderId} was accepted.");
        return Task.CompletedTask;
    }
}