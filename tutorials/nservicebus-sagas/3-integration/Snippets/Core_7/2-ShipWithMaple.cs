using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Maple
{
    // ShipWithMapleHandler snipopet located in solution

    class Program
    {
        static void Routing(TransportExtensions<LearningTransport> transport)
        {
            #region ShipWithMaple-Routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
            routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");

            #endregion
        }
    }

    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipmentAcceptedByMaple>
    {
        static ILog log = LogManager.GetLogger<ShipOrderWorkflow>();

        #region HandleShipOrder
        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            log.Info($"ShipOrderWorkflow for Order #{Data.OrderId} - Trying Maple first.");

            // Execute order to ship with Maple
            await context.Send(new ShipWithMaple() { OrderId = Data.OrderId });

            // Add timeout to escalate if Maple did not ship in time.
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
        }
        #endregion

        #region ShipWithMaple-ShipmentAccepted
        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Successfully shipped with Maple");

            Data.ShipmentAcceptedByMaple = true;

            MarkAsComplete();

            return Task.CompletedTask;
        }
        #endregion

        #region ShipWithMaple-Data
        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
        }
        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        #region ShippingEscalationTimeout
        internal class ShippingEscalation
        {
        }
        #endregion
    }
}
