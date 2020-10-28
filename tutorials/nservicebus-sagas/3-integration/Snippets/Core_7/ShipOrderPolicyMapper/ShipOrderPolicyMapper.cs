namespace Core_7.ShipOrderWorkflowMapper
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>
    {
        #region ShipOrderWorkflowMapper
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }
        #endregion

        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Add timeout to escalate if Maple did not ship in time.
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
        }

        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
            public bool ShipmentOrderSentToAlpine { get; set; }
            public bool ShipmentAcceptedByAlpine { get; set; }
        }

        internal class ShippingEscalation
        {

        }
    }

    internal class ShipOrder
    {
        public string OrderId { get; internal set; }
    }
}