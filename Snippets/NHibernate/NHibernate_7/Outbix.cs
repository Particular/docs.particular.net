namespace NHibernate_7
{
    using System;
    using NHibernate;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NServiceBus;
    using NServiceBus.NHibernate.Outbox;
    using NServiceBus.Outbox.NHibernate;

    public class Outbox
    {
        public void CustomMapping(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxNHibernateCustomMapping

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseOutboxRecord<MyOutboxRecord, MyOutboxRecordMapping>();

            #endregion
        }

        #region OutboxNHibernateCustomMapping
        public class MyOutboxRecord : IOutboxRecord
        {
            public virtual string MessageId { get; set; }
            public virtual bool Dispatched { get; set; }
            public virtual DateTime? DispatchedAt { get; set; }
            public virtual string TransportOperations { get; set; }
        }

        public class MyOutboxRecordMapping : ClassMapping<MyOutboxRecord>
        {
            public MyOutboxRecordMapping()
            {
                Table("MyOutboxTable");
                Id(x => x.MessageId, m => m.Generator(Generators.Assigned));
                Property(p => p.Dispatched, pm =>
                {
                    pm.Column(c => c.NotNullable(true));
                    pm.Index("OutboxRecord_Dispatched_Idx");
                });
                Property(p => p.DispatchedAt, pm => pm.Index("OutboxRecord_DispatchedAt_Idx"));
                Property(p => p.TransportOperations, pm => pm.Type(NHibernateUtil.StringClob));
            }
        }
        #endregion

    }
}