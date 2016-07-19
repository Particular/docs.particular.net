using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region AzureMultiHost_MessageMapping
public class ConfigureMessageEndpointMappings :
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
                    Endpoint = "Receiver",
                    Messages = "Ping, Shared"
                }
            }

        };
    }
}
#endregion
