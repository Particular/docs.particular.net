using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

class OrderMap :
    ClassMapping<Order>
{
    public OrderMap()
    {
        Table("Orders");
        Id(x => x.OrderId, m => m.Generator(Generators.Assigned));
        Property(p => p.Value);
    }
}