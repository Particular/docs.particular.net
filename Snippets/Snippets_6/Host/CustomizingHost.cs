namespace Snippets6.Host
{
    using NServiceBus;

    #region customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // To customize, use the configuration parameter. 
            // For example, to customize the container:
            endpointConfiguration.UseContainer<AutofacBuilder>();
        }
    }

    #endregion
}