namespace Sender
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    public class ConfigureMessageEndpointMappings : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                        Endpoint = "Receiver",
                        //AssemblyName = "Shared",
                        Messages = "Ping, Shared"
                    }
                }
            };
        }
    }
}