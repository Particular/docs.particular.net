using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Core_6
{
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

#pragma warning disable CS0162 // Unreachable code detected

        #region ThrowSystemic

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // This is normally where some business logic would occur

            throw new Exception("BOOM");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }

        #endregion

#pragma warning restore CS0162 // Unreachable code detected

        #region Random
        static Random random = new Random();
        #endregion

        public void ThrowTransient()
        {
            #region ThrowTransient
            if (random.Next(0, 5) == 0)
            {
                throw new Exception("Oops");
            }
            #endregion
        }
    }
}