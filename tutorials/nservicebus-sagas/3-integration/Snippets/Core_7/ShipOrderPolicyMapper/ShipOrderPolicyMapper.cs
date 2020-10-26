namespace Core_7.ShipOrderPolicyMapper
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    class ShipOrderPolicy :
        Saga<ShipOrderPolicy.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>
    {
        #region ShipOrderPolicyMapper
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(m => m.OrderId).ToSaga(s => s.OrderId);
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