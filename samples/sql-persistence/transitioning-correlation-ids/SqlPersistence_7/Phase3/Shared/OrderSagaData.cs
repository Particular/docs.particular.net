using System;
using NServiceBus;

#region sagadataPhase3

public class OrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
}
#endregion