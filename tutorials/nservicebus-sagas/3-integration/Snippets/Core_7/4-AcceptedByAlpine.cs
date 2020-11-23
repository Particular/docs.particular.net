using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace AlpineAccepted
{
    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipmentAcceptedByMaple>
    {
        static ILog log = LogManager.GetLogger<ShipOrderWorkflow>();

        public Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Stub
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            // Stub
            return Task.CompletedTask;
        }

        #region ShipmentAcceptedByAlpine
        public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Successfully shipped with Alpine");

            Data.ShipmentAcceptedByAlpine = true;

            MarkAsComplete();

            return Task.CompletedTask;
        }
        #endregion

        #region AcceptedByAlpine-Data
        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
            public bool ShipmentOrderSentToAlpine { get; set; }
            public bool ShipmentAcceptedByAlpine { get; set; }
        }
        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        internal class ShippingEscalation
        {
        }
    }
}
