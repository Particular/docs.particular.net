namespace Core_7.ShipmentAcceptedByMaple
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    
    class ShipmentAcceptedByMapleSnippet
    {
        #region ShipmentAcceptedByMaple
        public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Maple");

            Data.ShipmentAcceptedByMaple = true;

            return Task.CompletedTask;

        }
        #endregion
    }

    internal class log
    {
        internal static void Info(string v)
        {
            throw new NotImplementedException();
        }
    }

    internal class Data
    {
        private static string orderId;

        public static string OrderId { get => orderId; set => orderId = value; }

        public static bool ShipmentAcceptedByMaple { get; internal set; }
    }

    internal class ShipmentAcceptedByMaple
    {
        public string OrderId { get; internal set; }
    }
}