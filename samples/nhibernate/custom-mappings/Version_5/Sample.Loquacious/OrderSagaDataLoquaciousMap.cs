using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

public class OrderSagaDataLoquaciousMap : ClassMapping<OrderSagaDataLoquacious>
{
    public OrderSagaDataLoquaciousMap()
    {
        Console.WriteLine("\n\n\n\n\n\n======BOEM===========================================\n\n\n\n\n\n");
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
    }
}