using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningCommandsAs(type =>
        {
            return type.Namespace != null &&
                   type.Namespace.StartsWith("Store") &&
                   type.Namespace.EndsWith("Commands");
        });
        conventions.DefiningEventsAs(type =>
        {
            return type.Namespace != null &&
                   type.Namespace.StartsWith("Store") &&
                   type.Namespace.EndsWith("Events");
        });
        conventions.DefiningMessagesAs(type =>
        {
            return type.Namespace != null &&
                   type.Namespace.StartsWith("Store") &&
                   type.Namespace.EndsWith("RequestResponse");
        });
        conventions.DefiningEncryptedPropertiesAs(property =>
        {
            return property.Name.StartsWith("Encrypted");
        });
        endpointConfiguration.RijndaelEncryptionService();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
    }
}