namespace Core_7.Lesson1.OrderProcessing
{
#pragma warning disable 1998

    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;
    #region ShippingPolicyShipOrder
    public class ShipOrder : ICommand
    {
        public string OrderId { get; set; }
    }
    #endregion

    public class ShippingPolicyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }

    public class ShippingPolicy : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>,
            IAmStartedByMessages<OrderBilled>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {

        }

        #region ShippingPolicyProcessOrder
        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
                MarkAsComplete();
            }
        }
        #endregion

        #region ShippingPolicyFinalHandleWithProcessOrder
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"OrderPlaced message received.");
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"OrderBilled message received.");
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }
        #endregion
    }

    #region EmptyShipOrderHandler
    class ShipOrderHandler : IHandleMessages<ShipOrder>
    {
        static ILog log = LogManager.GetLogger<ShipOrderHandler>();

        public Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order [{message.OrderId}] - Successfully shipped.");
            return Task.CompletedTask;
        }
    }
    #endregion

#pragma warning restore 1998
}
