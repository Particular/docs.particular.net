using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

namespace DynamoDB_1;

public class SagaConfig
{
    void ConfigureSagaTable(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoSagaTableConfiguration
        dynamoConfig.Sagas().Table = new TableConfiguration
        {
            TableName = "MySagaTable",
            PartitionKeyName = "MySagaPartitionKey",
            SortKeyName = "MySagaSortKey"
        };
        #endregion
    }

    void PessimisticLocking(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBSagaPessimisticLocking

        dynamoConfig.Sagas().UsePessimisticLocking = true;

        #endregion

        #region DynamoDBLeaseDuration

        dynamoConfig.Sagas().LeaseDuration = TimeSpan.FromSeconds(15);

        #endregion

        #region DynamoDBLeaseAcquisitionTimeout

        dynamoConfig.Sagas().LeaseAcquisitionTimeout = TimeSpan.FromSeconds(5);

        #endregion
    }
}