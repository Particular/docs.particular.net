namespace Snippets5.Host_6.UpgradeGuides._6to7
{
    using NServiceBus;

    #region 6to7customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // perform some custom configuration
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
        }
    }

    #endregion
}