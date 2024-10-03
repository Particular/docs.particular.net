using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Billing
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
       private readonly ILogger<OrderPlacedHandler> logger;

        public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received OrderPlaced, OrderId = {orderId} - Charging credit card...", message.OrderId);

            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderBilled);
        }
    }
}