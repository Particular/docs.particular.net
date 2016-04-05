namespace Snippets5.Host_6.UpgradeGuides._6to7
{
    using NServiceBus;
    using NServiceBus.Persistence;

    #region 6to7customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // perform some custom configuration
            busConfiguration.UseContainer<AutofacBuilder>();
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<RavenDBPersistence>();
        }
    }

    #endregion
}