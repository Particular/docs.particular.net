using System;
using NServiceBus;

#region sagadataPhase2

public class OrderSagaData :
    ContainSagaData
{
    public int OrderNumber { get; set; }
    public Guid OrderId { get; set; }
}
#endregion