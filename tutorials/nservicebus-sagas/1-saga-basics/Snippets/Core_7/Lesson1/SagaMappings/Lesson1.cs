namespace Core_7.Lesson1.SagaMappings
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region ExtendedShippingPolicyData
    public class ShippingPolicyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }
    #endregion

    public class ShippingPolicy : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>, // This can start the saga
            IHandleMessages<OrderBilled>       // But surely, not this one!?
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        #region ShippingPolicyFinalMappings
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }
        #endregion

        #region ShippingPolicyCorrelationAutoPopulation
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            // DON'T NEED THIS! NServiceBus does this for us.
            Data.OrderId = message.OrderId;

            log.Info($"OrderPlaced message received.");
            Data.IsOrderPlaced = true;
            return Task.CompletedTask;
        }
        #endregion

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"OrderBilled message received.");
            return Task.CompletedTask;
        }
    }

#pragma warning restore 1998
}
