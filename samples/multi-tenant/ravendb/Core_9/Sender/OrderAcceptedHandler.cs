using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} accepted.", message.OrderId);
        return Task.CompletedTask;
    }
}