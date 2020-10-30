using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace AlpineAccepted
{
    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipmentAcceptedByMaple>
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

        #region ShipmentAcceptedByAlpine
        public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Alpine");

            Data.ShipmentAcceptedByAlpine = true;

            return Task.CompletedTask;
        }
        #endregion

        #region AcceptedByAlpine-Data
        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
            public bool ShipmentOrderSentToAlpine { get; set; }
            public bool ShipmentAcceptedByAlpine { get; set; }
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
