namespace Core_7.BuyersRemorseTimeoutRequest
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            
        }

        #region BuyersRemorseTimeoutRequest

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            Data.OrderId = message.OrderId;

            log.Info($"Starting cool down period for order #{Data.OrderId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        }

        #endregion
    }

    internal class BuyersRemorseIsOver
    {

    }

    public class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
    }

    internal class PlaceOrder
    {
        public string OrderId { get; set; }
    }
}
