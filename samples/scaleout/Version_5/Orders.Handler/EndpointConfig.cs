using NServiceBus;

namespace Orders.Handler
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.UsePersistence<InMemoryPersistence>();
        }
    }
    
}
