using System;
using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        var connectionString = "UseDevelopmentStorage=true";

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString(connectionString);
        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString(connectionString);
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        endpointConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}