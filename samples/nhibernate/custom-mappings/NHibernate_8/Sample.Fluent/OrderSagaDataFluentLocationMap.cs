using FluentNHibernate.Mapping;

public class OrderSagaDataFluentLocationMap :
    ClassMap<OrderSagaDataFluent.Location>
{
    public OrderSagaDataFluentLocationMap()
    {
        Table("OrderSagaDataFluent_Location");
        Id(x => x.Id)
            .GeneratedBy.GuidComb();
        Map(x => x.Long);
        Map(x => x.Lat);
    }
}