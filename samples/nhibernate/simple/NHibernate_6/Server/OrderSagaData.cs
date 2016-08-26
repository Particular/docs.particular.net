using System;
using NServiceBus.Saga;

#region sagadata
namespace Server
{
    public class OrderSagaData :
        ContainSagaData
    {
        public virtual Guid OrderId { get; set; }
        public virtual string OrderDescription { get; set; }
    }
}
#endregion