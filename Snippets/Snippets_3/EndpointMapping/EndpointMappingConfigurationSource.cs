using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using System.Configuration;
using NServiceBus;

#region endpoint-mapping-configurationprovider
public class EndpointMappingConfigurationSource : IConfigurationSource
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
            return config as T;
        }

        // To in app.config for other sections not defined in this method, otherwise return null.
        return ConfigurationManager.GetSection(typeof(T).Name) as T;
    }
}
#endregion

public class InjectEndpointMappingConfigurationSource 
{
    public void Init()
    {
        #region inject-endpoint-mapping-configuration-source
        Configure.With()
            .CustomConfigurationSource(new EndpointMappingConfigurationSource());
        #endregion
    }
}