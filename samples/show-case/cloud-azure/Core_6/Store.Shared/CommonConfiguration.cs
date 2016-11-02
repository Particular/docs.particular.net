using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>().ConnectionString("UseDevelopmentStorage=true");
        endpointConfiguration.UsePersistence<AzureStoragePersistence>().ConnectionString("UseDevelopmentStorage=true");
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}