using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using System.Configuration;

#region endpoint-mapping-configurationsource
public class EndpointMappingConfigurationProvider : 
    IProvideConfiguration<UnicastBusConfig>
{
    public UnicastBusConfig GetConfiguration()
    {
        //read from existing config 
        var config = (UnicastBusConfig) ConfigurationManager
            .GetSection(typeof(UnicastBusConfig).Name);
        if (config == null)
        {
            //create new config if it doent exist
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
