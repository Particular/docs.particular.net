using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

#pragma warning disable 162

namespace Sales
{
    public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
        IHandleMessages<PlaceOrder>
    {

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

            // This is normally where some business logic would occur

            // Uncomment to test throwing a systemic exception
            // throw new Exception("BOOM");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }
    }
}

#pragma warning restore 162
