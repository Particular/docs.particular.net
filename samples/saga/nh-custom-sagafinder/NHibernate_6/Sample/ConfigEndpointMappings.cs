using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigEndpointMappings :
    IProvideConfiguration<UnicastBusConfig>
{
	public UnicastBusConfig GetConfiguration()
	{
		return new UnicastBusConfig
		{
			MessageEndpointMappings = new MessageEndpointMappingCollection
			{
				new MessageEndpointMapping
				{
					AssemblyName = "Sample",
					Endpoint = "Samples.NHibernateCustomSagaFinder"
				}
			}
		};
	}
}
