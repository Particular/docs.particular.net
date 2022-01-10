using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        readonly OrderCalculator orderCalculator;
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public OrderPlacedHandler(OrderCalculator orderCalculator)
        {
            this.orderCalculator = orderCalculator;
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

            var orderBilled = new OrderBilled
            {
                CustomerId = message.CustomerId,
                OrderId = message.OrderId,
                OrderValue = orderCalculator.GetOrderTotal(message.OrderId)
            };
            return context.Publish(orderBilled);
        }
    }
}