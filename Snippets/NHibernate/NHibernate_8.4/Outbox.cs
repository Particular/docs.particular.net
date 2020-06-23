namespace NHibernate_7
{
    using NServiceBus;
    using NServiceBus.NHibernate.Outbox;

    public class Outbox
    {
        public void CustomTableName(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxNHibernateCustomTableNameConfig

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.CustomizeOutboxTableName(
                outboxTableName: "MyEndpointOutbox", 
                outboxSchemaName: "MySchema");

            #endregion
        }

        public void PessimisticMode(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxPessimisticMode

            var outboxSettings = endpointConfiguration.EnableOutbox();
            outboxSettings.UsePessimisticConcurrencyControl();

            #endregion
        }

        public void TransactionScopeMode(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxTransactionScopeMode

            var outboxSettings = endpointConfiguration.EnableOutbox();
            outboxSettings.UseTransactionScope();

            #endregion
        }
    }
}