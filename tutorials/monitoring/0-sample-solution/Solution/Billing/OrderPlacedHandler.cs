using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Billing
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderBilled);
        }
    }
}