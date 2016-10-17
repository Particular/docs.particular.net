using NServiceBus;

public static class ConfigHelper
{
    public static void SharedConfig(this EndpointConfiguration endpointConfiguration)
    {
        #region AddMessageBodyWriter

        endpointConfiguration.AddMessageBodyWriter();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
    }
}