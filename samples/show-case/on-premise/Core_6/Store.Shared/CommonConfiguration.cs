using System;
using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        endpointConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}