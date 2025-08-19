using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;


public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        context.MessageHeaders.TryGetValue("tenant_id", out var tenantId);

        logger.LogInformation("Tenant {TenantId} order {OrderId} was accepted.", tenantId, message.OrderId);
        return Task.CompletedTask;
    }
}