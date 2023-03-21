using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

namespace DynamoDB_1;

public class Customization
{
    void SharedTableConfig(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBTableCustomizationShared

        dynamoConfig.UseSharedTable(new TableConfiguration
        {
            TableName = "MyTable",
            PartitionKeyName = "MyPartitionKey",
            SortKeyName = "MySortKey"
        });

        #endregion
    }

    void SplitTableConfig(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBTableCustomizationSplit

        dynamoConfig.Sagas().Table = new TableConfiguration
        {
            TableName = "MySagaTable",
            PartitionKeyName = "MySagaPartitionKey",
            SortKeyName = "MySagaSortKey"
        };

        dynamoConfig.Outbox().Table = new TableConfiguration
        {
            TableName = "MyOutboxTable",
            PartitionKeyName = "MyOutboxPartitionKey",
            SortKeyName = "MyOutboxSortKey",
            TimeToLiveAttributeName = "MyOutboxTtlAttribute"
        };

        #endregion
    }
}