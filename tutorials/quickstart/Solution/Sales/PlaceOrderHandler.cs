using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales
{    
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static readonly Random random = new Random();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // This is normally where some business logic would occur

            #region ThrowTransientException
            // Uncomment to test throwing transient exceptions
            //if (random.Next(0, 5) == 0)
            //{
            //    throw new Exception("Oops");
            //}
            #endregion

            #region ThrowFatalException
            // Uncomment to test throwing fatal exceptions
            //throw new Exception("BOOM");
            #endregion

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };

            log.Info($"Publishing OrderPlaced, OrderId = {message.OrderId}");

            return context.Publish(orderPlaced);
        }
    }
}
