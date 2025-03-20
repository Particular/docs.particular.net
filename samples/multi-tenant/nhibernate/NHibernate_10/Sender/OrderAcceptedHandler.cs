using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;


public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
  
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        context.MessageHeaders.TryGetValue("tenant_id", out var tenantId);

        logger.LogInformation($"Tenant {tenantId} order {message.OrderId} was accepted.");
        return Task.CompletedTask;
    }
}