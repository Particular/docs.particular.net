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
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
            {
                log.Info($"Order [{Data.OrderId}] - We didn't receive answer from Maple, let's try Alpine.");
                Data.ShipmentOrderSentToAlpine = true;
                await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId })
                    .ConfigureAwait(false);
                await RequestTimeout(context, TimeSpan.FromSeconds(20),
                    new ShippingEscalation()).ConfigureAwait(false);
            }

            // No response from Maple nor Alpine
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentAcceptedByAlpine)
            {
                log.Warn($"Order [{Data.OrderId}] - " +
                    $"We didn't receive answer from either Maple nor Alpine. We need to escalate!");
                // escalate to Warehouse Manager!
                await context.Publish<ShipmentFailed>().ConfigureAwait(false);
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