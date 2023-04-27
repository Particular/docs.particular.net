using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

namespace DynamoDB_1;

public class Customization
{
    void SharedTableConfig(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBTableCustomizationShared

        persistence.UseSharedTable(new TableConfiguration
        {
            TableName = "MyTable",
            PartitionKeyName = "MyPartitionKey",
            SortKeyName = "MySortKey"
        });

        #endregion
    }

    void DisableTableCreation(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBDisableTableCreation

        persistence.DisableTablesCreation();

        #endregion
    }
}