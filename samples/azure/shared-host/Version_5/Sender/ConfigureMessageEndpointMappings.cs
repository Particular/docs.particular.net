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
                #region AzureMultiHost_MessageMapping
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                        Endpoint = "Receiver",
                        Messages = "Ping, Shared"
                    }
                }

                #endregion
            };
        }
    }
}