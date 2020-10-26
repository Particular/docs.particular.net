namespace Core_7.ShipmentAcceptedByMapleData
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    
    class ShipmentAcceptedByMapleData
    {
        #region ShipmentAcceptedByMapleData
        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool ShipmentAcceptedByMaple { get; set; }
        }
        #endregion
    }
}