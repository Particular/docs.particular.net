namespace DynamoDB_4;

using NServiceBus;

public class SagaConfig
{
    void ConfigureSagaTable(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoSagaTableConfiguration
        persistence.Sagas().Table = new TableConfiguration
        {
            TableName = "MySagaTable",
            PartitionKeyName = "MySagaPartitionKey",
            SortKeyName = "MySagaSortKey"
        };
        #endregion
    }

    void PessimisticLocking(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBSagaPessimisticLocking

        persistence.Sagas().UsePessimisticLocking = true;

        #endregion

        #region DynamoDBLeaseDuration

        persistence.Sagas().LeaseDuration = TimeSpan.FromSeconds(15);

        #endregion

        #region DynamoDBLeaseAcquisitionTimeout

        persistence.Sagas().LeaseAcquisitionTimeout = TimeSpan.FromSeconds(5);

        #endregion
    }
}