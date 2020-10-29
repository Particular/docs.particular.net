namespace Core_7.IfShipmentAccepted
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    
    class IfShipmentAccepted
    {
        void someMethod()
        {
            #region IfShipmentAccepted
            if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
            #endregion
            {
            }
        }
    }

    internal class Data
    {
        public static bool ShipmentAcceptedByMaple { get; internal set; }
        public static bool ShipmentOrderSentToAlpine { get; internal set; }
    }
}