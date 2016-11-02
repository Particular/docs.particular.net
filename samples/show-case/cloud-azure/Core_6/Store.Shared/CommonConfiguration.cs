using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>().ConnectionString("UseDevelopmentStorage=true");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}