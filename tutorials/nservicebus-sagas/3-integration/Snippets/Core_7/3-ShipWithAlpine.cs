using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Alpine
{
    // ShipWithMapleHandler snipopet located in solution

    class Program
    {
        static void Routing(TransportExtensions<LearningTransport> transport)
        {
            #region ShipWithAlpine-Routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
            routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");
            routing.RouteToEndpoint(typeof(ShipWithAlpine), "Shipping");
            #endregion
        }
    }

    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipmentAcceptedByMaple>,
        IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
    {
        static ILog log = LogManager.GetLogger<ShipOrderWorkflow>();

        public Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Stub only
            return Task.CompletedTask;
        }

        #region ShipWithMaple-ShipmentAcceptedRevision
        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            if (!Data.ShipmentOrderSentToAlpine)
            {
                log.Info($"Order [{Data.OrderId}] - Successfully shipped with Maple");

                Data.ShipmentAcceptedByMaple = true;

                MarkAsComplete();
            }

            return Task.CompletedTask;
        }
        #endregion

        #region ShippingEscalation
        public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
        {
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
            {
                log.Info($"Order [{Data.OrderId}] - We didn't receive answer from Maple, let's try Alpine.");
                Data.ShipmentOrderSentToAlpine = true;
                await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
                await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
            }
        }
        #endregion

        #region ShipWithAlpine-Data
        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
            public bool ShipmentOrderSentToAlpine { get; set; }
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
