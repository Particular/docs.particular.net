namespace Core_7.ShippingEscalation
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    
    class ShippingEscalation : Saga<Data>
    {
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
            // ...
        #endregion
            // No response from Maple nor Alpine
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentAcceptedByAlpine)
            {
                log.Warn($"Order [{Data.OrderId}] - We didn't receive answer from either Maple nor Alpine. We need to escalate!");
                // escalate to Warehouse Manager!
                await context.Publish<ShipmentFailed>();
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<Data> mapper)
        {
            throw new NotImplementedException();
        }
    }

    internal class ShipmentFailed
    {
    }

    internal class ShipWithAlpine
    {
        public ShipWithAlpine()
        {
        }

        public object OrderId { get; set; }
    }

    internal class log
    {
        internal static void Info(string v)
        {
            throw new NotImplementedException();
        }

        internal static void Warn(string v)
        {
            throw new NotImplementedException();
        }
    }

    internal class Data:ContainSagaData
    {
        public static bool ShipmentAcceptedByMaple { get; internal set; }
        public static bool ShipmentOrderSentToAlpine { get; internal set; }
        public static object OrderId { get; internal set; }
        public bool ShipmentAcceptedByAlpine { get; internal set; }
    }
}