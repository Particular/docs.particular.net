using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Sales
{    
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        static readonly Random random = new Random();
        private readonly ILogger<PlaceOrderHandler> logger;

        public PlaceOrderHandler(ILogger<PlaceOrderHandler> logger)
        {
            this.logger = logger;
        }        

         public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received PlaceOrder, OrderId = {orderId}",  message.OrderId);

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

            logger.LogInformation("Publishing OrderPlaced, OrderId = {orderId}", message.OrderId);

            return context.Publish(orderPlaced);
        }
    }
}
