namespace Snippets5.Host_6
{
    using NServiceBus;

    #region customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // To customize, use the configuration parameter.
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
        }
    }

    #endregion
}