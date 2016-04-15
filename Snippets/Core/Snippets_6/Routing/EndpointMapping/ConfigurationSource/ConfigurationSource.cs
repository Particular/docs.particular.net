namespace Core6.Routing.EndpointMapping.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region endpoint-mapping-configurationsource
    public class ConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(UnicastBusConfig))
            {
                //read from existing config 
                UnicastBusConfig config = (UnicastBusConfig)ConfigurationManager
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
                return config as T;
            }

            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}