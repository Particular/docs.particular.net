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
            #region OutboxNHibernateCustomMappingConfig

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.UseOutboxRecord<MyOutboxRecord, MyOutboxRecordMapping>();

            #endregion
        }

        #region OutboxNHibernateCustomMapping

        public class MyOutboxRecord :
            IOutboxRecord
        {
            public virtual string MessageId { get; set; }
            public virtual bool Dispatched { get; set; }
            public virtual DateTime? DispatchedAt { get; set; }
            public virtual string TransportOperations { get; set; }
        }

        public class MyOutboxRecordMapping :
            ClassMapping<MyOutboxRecord>
        {
            public MyOutboxRecordMapping()
            {
                Table("MyOutboxTable");
                Id(
                    idProperty: record => record.MessageId,
                    idMapper: mapper => mapper.Generator(Generators.Assigned));
                Property(
                    property: record => record.Dispatched,
                    mapping: mapper =>
                    {
                        mapper.Column(c => c.NotNullable(true));
                        mapper.Index("OutboxRecord_Dispatched_Idx");
                    });
                Property(
                    property: record => record.DispatchedAt,
                    mapping: pm => pm.Index("OutboxRecord_DispatchedAt_Idx"));
                Property(
                    property: record => record.TransportOperations,
                    mapping: mapper => mapper.Type(NHibernateUtil.StringClob));
            }
        }

        #endregion

    }
}