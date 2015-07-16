using FluentNHibernate.Mapping;

public class OrderSagaDataFluentMap : ClassMap<OrderSagaDataFluent>
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
            .Not.Nullable();
        Version(x => x.Version);
    }
}