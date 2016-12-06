using System;
using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString");
        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString(connectionString);
        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString(connectionString);
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}