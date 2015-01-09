using NServiceBus;

namespace HostProfileLogging_5
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseSerialization<JsonSerializer>();
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
        }
    }
}
