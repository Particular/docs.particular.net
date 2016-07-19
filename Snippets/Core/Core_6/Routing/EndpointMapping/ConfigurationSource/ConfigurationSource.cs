namespace Core6.Routing.EndpointMapping.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region endpoint-mapping-configurationsource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(UnicastBusConfig))
            {
                //read from existing config
                var config = (UnicastBusConfig)ConfigurationManager
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
                var endpointMapping = new MessageEndpointMapping
                {
                    AssemblyName = "assembly",
                    Endpoint = "queue@machinename"
                };
                config.MessageEndpointMappings.Add(endpointMapping);
                return config as T;
            }

            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}