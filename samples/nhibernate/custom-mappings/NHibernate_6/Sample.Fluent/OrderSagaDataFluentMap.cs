using FluentNHibernate.Mapping;

public class OrderSagaDataFluentMap :
    ClassMap<OrderSagaDataFluent>
{
    public OrderSagaDataFluentMap()
    {
        Id(x => x.Id)
            .GeneratedBy
            .Assigned();
        Map(x => x.OriginalMessageId);
        Map(x => x.Originator);
        Map(x => x.OrderId)
            .CustomType("AnsiString")
            .Length(100)
            .Not.Nullable()
            .Unique();
        Version(x => x.Version);
        References(x => x.From, "FromLocation")
            .Cascade.All();
        References(x => x.To, "ToLocation")
            .Cascade.All();
        Component(x => x.Total, c =>
        {
            c.Map(x => x.Amount);
            c.Map(x => x.Currency).Length(3).CustomType("AnsiString");
        });
    }
}