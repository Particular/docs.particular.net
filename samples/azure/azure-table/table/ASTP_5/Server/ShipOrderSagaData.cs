using System;
using NServiceBus;

#region shipordersagadata

public class ShipOrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
}
#endregion