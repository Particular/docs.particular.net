namespace Core_7.BuyersRemorsePolicyMapping
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    #region BuyersRemorsePolicyMapping

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            //To be replaced with business code
            return Task.CompletedTask;
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
