namespace Snippets6.Host
{
    using NServiceBus;

    #region customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration configuration)
        {
            // To customize, use the configuration parameter. 
            // For example, to customize the endpoint name:
            configuration.EndpointName("NewEndpointName");
        }
    }

    #endregion
}