using NServiceBus;

namespace Orders.Sender
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.UsePersistence<InMemoryPersistence>();
        }
    }
}
