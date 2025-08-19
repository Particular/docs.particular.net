using NHibernate.Mapping.ByCode.Conformist;

public class OrderShippedMap :
    ClassMapping<OrderShipped>
{
    public OrderShippedMap()
    {
        Id(x => x.Id);
        Property(x => x.ShippingDate);
    }
}