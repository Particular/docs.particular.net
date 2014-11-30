
namespace Snippets_5.Host
{
    #region customize_nsb_host_v5
    using NServiceBus;
    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            // To customize, use the configuration parameter. 
            // For example, to customize the endpoint name:
            configuration.EndpointName("NewEndpointName");
        }
    }
    #endregion
}
