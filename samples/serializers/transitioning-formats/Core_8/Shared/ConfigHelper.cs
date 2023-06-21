using NServiceBus;

public static class ConfigHelper
{
    public static void SharedConfig(this EndpointConfiguration endpointConfiguration)
    {
        #region AddMessageBodyWriter

        endpointConfiguration.AddMessageBodyWriter();

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
    }
}
