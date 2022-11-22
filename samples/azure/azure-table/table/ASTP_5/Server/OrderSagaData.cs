using System;
using NServiceBus;

#region ordersagadata

public class OrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
}
#endregion