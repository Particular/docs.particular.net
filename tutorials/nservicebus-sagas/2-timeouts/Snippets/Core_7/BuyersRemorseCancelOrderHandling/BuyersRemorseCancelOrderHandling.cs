namespace Core_7.BuyersRemorseCancelOrderHandling
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

    #region BuyersRemorseCancelOrderHandling

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<BuyersRemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<CancelOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Update status in database?

            MarkAsComplete();

            return Task.CompletedTask;
        }
    }

    #endregion

    internal class OrderPlaced
    {
        public object OrderId { get; set; }
    }

    internal class BuyersRemorseIsOver
    {
    }

    internal class PlaceOrder
    {
        public object OrderId { get; internal set; }
    }

    internal class BuyersRemorseState
    {
        public object OrderId { get; set; }
    }

    internal class CancelOrder
    {
        public object OrderId { get; set; }
    }
}
