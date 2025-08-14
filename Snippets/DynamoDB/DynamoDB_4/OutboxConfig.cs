namespace DynamoDB_4;

using NServiceBus;

public class OutboxConfig
{
    void ConfigureSagaTable(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoOutboxTableConfiguration

        var outboxConfiguration = endpointConfiguration.EnableOutbox();
        outboxConfiguration.UseTable(new TableConfiguration
        {
            TableName = "MyOutboxTable",
            PartitionKeyName = "MyOutboxPartitionKey",
            SortKeyName = "MyOutboxSortKey",
            TimeToLiveAttributeName = "MyOutboxTtlAttribute"
        });
        #endregion
    }

    void CleanupConfig(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBOutboxCleanup

        var outboxConfiguration = endpointConfiguration.EnableOutbox();
        outboxConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(14));

        #endregion
    }
}