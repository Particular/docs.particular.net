using System;
using NServiceBus;

#region sagadata
public class OrderSagaData :
    ContainSagaData
{
    public virtual Guid OrderId { get; set; }
    public virtual string OrderDescription { get; set; }
}
#endregion