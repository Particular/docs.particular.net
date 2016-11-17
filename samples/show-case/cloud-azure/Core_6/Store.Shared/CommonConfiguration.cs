using System;
using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString");
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>().ConnectionString(connectionString);
        endpointConfiguration.UsePersistence<AzureStoragePersistence>().ConnectionString(connectionString);
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}