using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using Messages;

namespace Core_7.BuyersRemorseTimeoutRequest
{
    class BuyersRemorsePolicy
        : Saga<BuyersRemorseState>
        , IAmStartedByMessages<PlaceOrder>
        , IHandleMessages<CancelOrder>
        , IHandleTimeouts<BuyersRemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
            mapper.ConfigureMapping<CancelOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            Data.OrderId = message.OrderId;
            Data.CustomerId = message.CustomerId;

            log.Info($"Starting cool down period for order #{Data.OrderId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        }

        public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for order #{Data.OrderId} has elapsed.");
            var orderPlaced = new OrderPlaced
            {
                CustomerId = Data.CustomerId,
                OrderId = Data.OrderId
            };

            await context.Publish(orderPlaced);

            MarkAsComplete();
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Possibly publish an OrderCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }
    }

    internal class BuyersRemorseIsOver
    {
    }

    public class BuyersRemorseState : 
        ContainSagaData
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
    }
}