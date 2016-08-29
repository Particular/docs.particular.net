using System;

namespace Server
{
    public class OrderShipped
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime ShippingDate { get; set; }
    }
}