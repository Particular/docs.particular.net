
namespace Core3.Routing.EndpointMapping
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region endpoint-mapping-configurationprovider

    public class ProvideConfiguration :
        IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            //read from existing config
            var config = (UnicastBusConfig) ConfigurationManager
                .GetSection(typeof(UnicastBusConfig).Name);
            if (config == null)
            {
                //create new config if it doesn't exist
                config = new UnicastBusConfig
                {
                    MessageEndpointMappings = new MessageEndpointMappingCollection()
                };
            }
            //append mapping to config
            config.MessageEndpointMappings.Add(
                new MessageEndpointMapping
                {
                    AssemblyName = "assembly",
                    Endpoint = "queue@machinename"
                });
            return config;
        }
    }

    #endregion

}

