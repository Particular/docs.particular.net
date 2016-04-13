namespace Snippets6.Host_7
{
    using NServiceBus;

    #region customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // To customize, use the configuration parameter.
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
        }
    }

    #endregion
}