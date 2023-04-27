using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

namespace DynamoDB_1;

public class OutboxConfig
{
    void ConfigureSagaTable(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoOutboxTableConfiguration
        persistence.Outbox().Table = new TableConfiguration
        {
            TableName = "MyOutboxTable",
            PartitionKeyName = "MyOutboxPartitionKey",
            SortKeyName = "MyOutboxSortKey",
            TimeToLiveAttributeName = "MyOutboxTtlAttribute"
        };
        #endregion
    }

    void CleanupConfig(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBOutboxCleanup

        persistence.Outbox().TimeToLive = TimeSpan.FromDays(14);

        #endregion
    }


}