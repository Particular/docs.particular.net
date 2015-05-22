namespace Receiver
{
    using NServiceBus;
    using NServiceBus.Logging;
    using Shared;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.UseTransport<AzureStorageQueueTransport>()
                .ConnectionString("UseDevelopmentStorage=true");

            LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

            NServiceBusFeatures.DisableNotUsedFeatures(configuration);
        }
    }
}
