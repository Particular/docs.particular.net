namespace Core_7.ShipmentAcceptedByAlpine
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    
    class ShipmentAcceptedByAlpineSnippet
    {
        #region ShipmentAcceptedByAlpine
        public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
        {
            log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Alpine");

            Data.ShipmentAcceptedByAlpine = true;

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

        internal static void Warn(string v)
        {
            throw new NotImplementedException();
        }
    }

    internal class Data
    {
        public static object OrderId { get; internal set; }
        public static bool ShipmentAcceptedByAlpine { get; internal set; }
    }

    internal class ShipmentAcceptedByAlpine
    { 
    }
}