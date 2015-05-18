using NServiceBus;

namespace Store.Sales
{
    using Store.Shared;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.ApplyCommonConfiguration();
        }
    }

}