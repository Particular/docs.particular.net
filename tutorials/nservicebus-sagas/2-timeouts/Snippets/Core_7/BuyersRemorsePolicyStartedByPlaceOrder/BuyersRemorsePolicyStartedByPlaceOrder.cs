#pragma warning disable NSB0006 // Message that starts the saga does not have a message mapping

namespace Core_7.BuyersRemorsePolicyStartedByPlaceOrder
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    #region BuyersRemorsePolicyStartedByPlaceOrder

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            Data.OrderId = message.OrderId;

            return Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            // TO BE IMPLEMENTED
        }
    }

    #endregion

    internal class PlaceOrder
    {
        public string OrderId { get; set; }
    }

    internal class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
    }
}

#pragma warning restore NSB0006 // Message that starts the saga does not have a message mapping