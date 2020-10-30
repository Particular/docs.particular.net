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

        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Execute order to ship with Maple
            await context.Send(new ShipWithMaple() { OrderId = Data.OrderId })
                .ConfigureAwait(false);

            // Add timeout to escalate if Maple did not ship in time.
            await RequestTimeout(context, TimeSpan.FromSeconds(20),
                new ShippingEscalation()).ConfigureAwait(false);
        }

        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Maple");

            Data.ShipmentAcceptedByMaple = true;

            return Task.CompletedTask;
        }

        #region ShippingEscalation
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
