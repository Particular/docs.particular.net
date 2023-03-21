using NServiceBus;

namespace DynamoDB_1;

public class OutboxConfig
{
    void CleanupConfig(PersistenceExtensions<DynamoDBPersistence> dynamoConfig)
    {
        #region DynamoDBOutboxCleanup

        dynamoConfig.Outbox().TimeToLive = TimeSpan.FromDays(14);

        #endregion
    }
}