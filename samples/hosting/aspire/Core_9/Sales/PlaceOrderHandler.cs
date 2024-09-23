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

            // Force a transient exception to demonstrate failures in telemetry data
            if (random.Next(0, 5) == 0)
            {
                throw new Exception("Oops");
            }

            var publishOptions = new PublishOptions();
            // https://docs.particular.net/nservicebus/operations/opentelemetry#traces-emitted-span-structure-publish-operations
            publishOptions.ContinueExistingTraceOnReceive();

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced, publishOptions);
        }
    }
}