namespace Core_7.BuyersRemorseTimeoutRequest
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;
    using Messages;

    class BuyersRemorsePolicy
        : Saga<BuyersRemorseState>
        , IAmStartedByMessages<PlaceOrder>
        , IHandleMessages<CancelOrder>
        , IHandleTimeouts<BuyersRemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<CancelOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            Data.CustomerId = message.CustomerId;

            log.Info($"Starting cool down period for order #{Data.OrderId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        }

        public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
        {
            var orderPlaced = new OrderPlaced
            {
                OrderId = Data.OrderId
            };

            // TODO: Save order state in database?

            await context.Publish(orderPlaced);

            MarkAsComplete();
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Update status in database?

            MarkAsComplete();

            return Task.CompletedTask;
        }
    }

    internal class BuyersRemorseIsOver
    {

    }

    public class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }

    internal class PlaceOrder
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }
}