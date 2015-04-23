namespace Ordering.Client
{
    using NServiceBus;
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
	    public void Customize(BusConfiguration busConfiguration)
	    {
	        busConfiguration.UsePersistence<InMemoryPersistence>();
	    }
    }
}