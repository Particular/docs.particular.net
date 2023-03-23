using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

namespace DynamoDB_1;

public class OutboxConfig
{
    void ConfigureSagaTable(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoOutboxTableConfiguration
        dynamoConfig.Outbox().Table = new TableConfiguration
        {
            TableName = "MyOutboxTable",
            PartitionKeyName = "MyOutboxPartitionKey",
            SortKeyName = "MyOutboxSortKey",
            TimeToLiveAttributeName = "MyOutboxTtlAttribute"
        };
        #endregion
    }

    void CleanupConfig(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBOutboxCleanup

        dynamoConfig.Outbox().TimeToLive = TimeSpan.FromDays(14);

        #endregion
    }


}