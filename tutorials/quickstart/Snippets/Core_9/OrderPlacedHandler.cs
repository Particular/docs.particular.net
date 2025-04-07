#region OrderPlacedHandler

using System.Threading.Tasks;
using NServiceBus;
using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) :
    IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation(
            "Shipping has received OrderPlaced, OrderId = {orderId}", message.OrderId);
        return Task.CompletedTask;
    }
}

#endregion