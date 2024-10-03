using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Sales
{
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        private readonly ILogger<PlaceOrderHandler> logger;

        public PlaceOrderHandler(ILogger<PlaceOrderHandler> logger)
        {
            this.logger = logger;
        }

        #region UpdatedHandler

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

            // This is normally where some business logic would occur

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }

        #endregion
    }
}
