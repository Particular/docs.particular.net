namespace Shipping
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;
    using Messages;

    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipmentAcceptedByMaple>,
        IHandleMessages<ShipmentAcceptedByAlpine>,
        IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
    {
        static ILog log = LogManager.GetLogger<ShipOrderWorkflow>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
        }

        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            // Execute order to ship with Maple
            await context.Send(new ShipWithMaple() { OrderId = Data.OrderId });

            // Add timeout to escalate if Maple did not ship in time.
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
        }

        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Maple");

            Data.ShipmentAcceptedByMaple = true;

            return Task.CompletedTask;
        }

        public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Alpine");

            Data.ShipmentAcceptedByAlpine = true;

            return Task.CompletedTask;
        }

        public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
        {
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
            {
                log.Info($"Order [{Data.OrderId}] - We didn't receive answer from Maple, let's try Alpine.");
                Data.ShipmentOrderSentToAlpine = true;
                await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
                await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
            }

            // No response from Maple nor Alpine
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentAcceptedByAlpine)
            {
                log.Warn($"Order [{Data.OrderId}] - We didn't receive answer from either Maple nor Alpine. We need to escalate!");
                // escalate to Warehouse Manager!
                await context.Publish<ShipmentFailed>();
            }
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
}