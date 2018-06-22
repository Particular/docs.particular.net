namespace Core_7.Lesson1.StartingTheSaga
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

#pragma warning disable 1998

    namespace StartedBy1Message
    {
        #region ShippingPolicyStartedBy1Message
        public class ShippingPolicy : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>, // This can start the saga
            IHandleMessages<OrderBilled>       // But surely, not this one!?
        #endregion
        {
            static ILog log = LogManager.GetLogger<ShippingPolicy>();


            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
            {
                // TODO
            }

            public Task Handle(OrderPlaced message, IMessageHandlerContext context)
            {
                log.Info($"OrderPlaced message received.");
                return Task.CompletedTask;
            }

            public Task Handle(OrderBilled message, IMessageHandlerContext context)
            {
                log.Info($"OrderBilled message received.");
                return Task.CompletedTask;
            }
        }
    }

    namespace StartedBy2Messages
    {
        #region ShippingPolicyStartedBy2Messages
        public class ShippingPolicy : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>, // I can start the saga!
            IAmStartedByMessages<OrderBilled>  // I can start the saga too!
        {
            #endregion
            static ILog log = LogManager.GetLogger<ShippingPolicy>();


            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
            {
                // TODO
            }

            public Task Handle(OrderPlaced message, IMessageHandlerContext context)
            {
                log.Info($"OrderPlaced message received.");
                return Task.CompletedTask;
            }

            public Task Handle(OrderBilled message, IMessageHandlerContext context)
            {
                log.Info($"OrderBilled message received.");
                return Task.CompletedTask;
            }
        }
    }

#pragma warning restore 1998
}
