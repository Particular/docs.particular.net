using NServiceBus;

public static class ConfigHelper
{
    public static void SharedConfig(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.AddMessageBodyWriter();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
    }
}