using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.Conventions()
            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("Events"))
            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("RequestResponse"))
            .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.EnableInstallers();
    }
}