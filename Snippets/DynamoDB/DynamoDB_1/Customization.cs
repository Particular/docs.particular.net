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

    void DisableTableCreation(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBDisableTableCreation

        dynamoConfig.DisableTablesCreation();

        #endregion
    }
}