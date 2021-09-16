using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

public class OrderSagaDataLoquaciousMap :
    ClassMapping<OrderSagaDataLoquacious>
{
    public OrderSagaDataLoquaciousMap()
    {
        Id(x => x.Id, m => m.Generator(Generators.Assigned));
        Property(x => x.OriginalMessageId);
        Property(x => x.Originator);
        Property(x => x.OrderId, m =>
        {
            m.Unique(true);
            m.Length(100);
            m.NotNullable(true);
            m.Type(NHibernateUtil.AnsiString);
        });
        Version(x => x.Version, m => { });
        ManyToOne(x => x.From, m =>
        {
            m.Column("FromLocation");
            m.Cascade(Cascade.All | Cascade.DeleteOrphans);
        });
        ManyToOne(x => x.To, m =>
        {
            m.Column("ToLocation");
            m.Cascade(Cascade.All | Cascade.DeleteOrphans);
        });
        Component(x => x.Total, c =>
        {
            c.Property(x => x.Currency, m =>
            {
                m.Length(3);
                m.Type(NHibernateUtil.AnsiString);
            });
            c.Property(x => x.Amount);
        });
    }
}

public class OrderSagaDataLoquaciousLocationMap :
    ClassMapping<OrderSagaDataLoquacious.Location>
{
    public OrderSagaDataLoquaciousLocationMap()
    {
        Table("OrderSagaDataLoquacious_Location");
        Id(x => x.Id, m => m.Generator(Generators.GuidComb));
        Property(x => x.Long);
        Property(x => x.Lat);
    }
}
