using System.Threading.Tasks;
using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;
using Messages.Events;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static readonly ILog logger = LogManager.GetLogger<PlaceOrderHandler>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // This is normally where some database logic would occur

            var orderPlaced = new OrderPlaced {OrderId = message.OrderId};
            return context.Publish(orderPlaced);
        }
    }
}
