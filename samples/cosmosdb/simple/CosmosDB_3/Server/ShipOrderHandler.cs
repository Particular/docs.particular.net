using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    private readonly ILogger<ShipOrderHandler> logger;

    public ShipOrderHandler(ILogger<ShipOrderHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order Shipped. OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}