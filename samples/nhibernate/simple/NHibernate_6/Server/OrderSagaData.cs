using System;
using NServiceBus.Saga;

#region sagadata
public class OrderSagaData :
    ContainSagaData
{
    public virtual Guid OrderId { get; set; }
    public virtual string OrderDescription { get; set; }
}
#endregion