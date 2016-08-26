using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace Server
{
    public class OrderShipped
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime ShippingDate { get; set; }
    }

    public class OrderShippedMap : ClassMapping<OrderShipped>
    {
        public OrderShippedMap()
        {
            Id(x => x.Id);
            Property(x => x.ShippingDate);
        }
    }
}