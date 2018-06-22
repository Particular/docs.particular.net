namespace Core_7.Lesson1.SagaState
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region ShippingPolicyAugmentedWithData
    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IHandleMessages<OrderPlaced>,
        IHandleMessages<OrderBilled>
    #endregion
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        #region EmptyConfigureHowToFindSaga
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            // TODO
        }
        #endregion

        #region HandleBasicImplementation
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"OrderPlaced message received.");
            Data.IsOrderPlaced = true;
            return Task.CompletedTask;
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"OrderBilled message received.");
            Data.IsOrderBilled = true;
            return Task.CompletedTask;
        }
        #endregion
    }

#pragma warning restore 1998
}
