namespace DynamoDB_4;

using Amazon.DynamoDBv2;
using Amazon.Runtime;
using NServiceBus;

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

    void ThrottlingConfig(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBConfigureThrottlingWithClientConfig

        var dynamoDbClient = new AmazonDynamoDBClient(
            new AmazonDynamoDBConfig
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Adaptive,
                MaxErrorRetry = 3
            });
        persistence.DynamoClient(dynamoDbClient);

        #endregion
    }
}