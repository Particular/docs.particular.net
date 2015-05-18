namespace Store.CustomerRelations
{
    using NServiceBus;
    using Store.Shared;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.ApplyCommonConfiguration();
        }
    }
}