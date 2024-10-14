using Messages;
using NServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Shipping;

class ShipOrderHandler(ILogger<ShipOrderHandler> logger) : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{OrderId}] - Successfully shipped.", message.OrderId);
        return Task.CompletedTask;
    }
}
