namespace Core_7.BuyersRemoreseTimeoutHandling
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    #region BuyersRemorseTimeoutHandling

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>, 
        IHandleTimeouts<BuyersRemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            //Omitted for clarity
        }

        public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for order #{Data.OrderId} has elapsed.");

            var orderPlaced = new OrderPlaced
            {
                OrderId = Data.OrderId
            };

            await context.Publish(orderPlaced);

            MarkAsComplete();
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
    }

    internal class BuyersRemorseState : ContainSagaData
    {
        public object OrderId { get; set; }
    }
}
