using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace EdgeCases
{
    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
    {
        static ILog log = LogManager.GetLogger<ShipOrderWorkflow>();

        #region EdgeCases-ShipmentFailed
        public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
        {
            if (!Data.ShipmentAcceptedByMaple)
            {
                if (!Data.ShipmentOrderSentToAlpine)
                {
                    log.Info($"Order [{Data.OrderId}] - No answer from Maple, let's try Alpine.");
                    Data.ShipmentOrderSentToAlpine = true;
                    await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
                    await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
                }
                else if (!Data.ShipmentAcceptedByAlpine) // No response from Maple nor Alpine
                {
                    log.Warn($"Order [{Data.OrderId}] - No answer from Maple/Alpine. We need to escalate!");

                    // escalate to Warehouse Manager!
                    await context.Publish<ShipmentFailed>();

                    MarkAsComplete();
                }
            }
        }
        #endregion

        void FakeMethod()
        {
            #region EdgeCases-IfShipmentAccepted
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
            #endregion
            {
            }
        }

        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
            public bool ShipmentOrderSentToAlpine { get; set; }
            public bool ShipmentAcceptedByAlpine { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        internal class ShippingEscalation
        {
        }
    }
}