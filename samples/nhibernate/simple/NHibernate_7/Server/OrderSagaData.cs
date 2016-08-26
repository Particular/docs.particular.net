using System;
using NServiceBus;

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