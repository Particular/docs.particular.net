using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class ShipOrderHandler(ILogger<ShipOrderHandler> logger) :
    IHandleMessages<ShipOrder>
{

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order Shipped. OrderId {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}