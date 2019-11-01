using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

#pragma warning disable 162

namespace Sales
{
    using System;

    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static Random random = new Random();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // This is normally where some business logic would occur

            // Uncomment to test throwing a systemic exception
            //throw new Exception("BOOM");

            // Uncomment to test throwing a transient exception
            //if (random.Next(0, 5) == 0)
            //{
            //    throw new Exception("Oops");
            //}

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }
    }
}