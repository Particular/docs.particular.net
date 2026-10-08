using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class ShipOrderHandler(ILogger<ShipOrderHandler> logger) :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order Shipped. OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}