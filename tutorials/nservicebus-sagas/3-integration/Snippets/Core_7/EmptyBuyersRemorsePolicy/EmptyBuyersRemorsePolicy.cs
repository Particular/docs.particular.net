namespace Core_7.EmptyBuyersRemorsePolicy
{
    using NServiceBus;
    using NServiceBus.Logging;

#pragma warning disable 1998

    #region EmptyBuyersRemorsePolicy
    class BuyersRemorsePolicy : Saga<BuyersRemorseState>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            // TO BE IMPLEMENTED
        }
    }

    public class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
    }
    #endregion

#pragma warning restore 1998
}
