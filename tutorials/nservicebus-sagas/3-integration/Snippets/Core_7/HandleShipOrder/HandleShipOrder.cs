namespace Core_7.HandleShipOrder
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    class HandleShipOrder :
            Saga<HandleShipOrder.ShipOrderData>,
            IAmStartedByMessages<ShipOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        #region HandleShipOrder
        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Execute order to ship with Maple
            await context.Send(new ShipWithMaple() { OrderId = Data.OrderId })
                .ConfigureAwait(false);

            // Add timeout to escalate if Maple did not ship in time.
            await RequestTimeout(context, TimeSpan.FromSeconds(20), 
                new ShippingEscalation()).ConfigureAwait(false);
        }
        #endregion

        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
        }

        internal class ShippingEscalation
        {
        }
    }

    internal class ShipWithMaple
    {
        public ShipWithMaple()
        {
        }

        public string OrderId { get; set; }
    }

    internal class ShipOrder
    {
        public object OrderId { get; internal set; }
    }
}