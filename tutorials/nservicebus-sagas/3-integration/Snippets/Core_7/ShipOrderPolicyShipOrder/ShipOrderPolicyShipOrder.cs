namespace Core_7.ShipOrderWorkflowShipOrder
{
    using NServiceBus;
    using System.Threading.Tasks;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    #region ShipOrderWorkflowShipOrder
    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>
    {
        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
        }

        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
        }
        // ...
        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }
    }

    internal class ShipOrder
    {
        public string OrderId { get; internal set; }
    }
    #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}