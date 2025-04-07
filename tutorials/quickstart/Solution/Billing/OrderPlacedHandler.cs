using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Billing
{
    public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger):
        IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("Billing has received OrderPlaced, OrderId = {orderId}", message.OrderId);
            return Task.CompletedTask;
        }
    }
}