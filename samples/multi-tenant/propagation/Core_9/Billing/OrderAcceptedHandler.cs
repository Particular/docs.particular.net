using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        var tenant = context.MessageHeaders["tenant_id"];

        logger.LogInformation("Processing OrderAccepted message for tenant {Tenant}", tenant);

        return Task.CompletedTask;
    }
}
