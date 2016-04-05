namespace Snippets6.Host_7.UpgradeGuides._6to7
{
    using NServiceBus;

    #region 6to7customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // perform some custom configuration
            endpointConfiguration.UseContainer<AutofacBuilder>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<RavenDBPersistence>();
        }
    }

    #endregion
}