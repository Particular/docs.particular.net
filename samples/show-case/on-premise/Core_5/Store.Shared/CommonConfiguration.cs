using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this BusConfiguration busConfiguration)
    {
        busConfiguration.UseTransport<MsmqTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var conventions = busConfiguration.Conventions();
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
        busConfiguration.RijndaelEncryptionService();
        busConfiguration.EnableInstallers();
    }
}