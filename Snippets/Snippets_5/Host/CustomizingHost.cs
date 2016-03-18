namespace Snippets5.Host
{
    using NServiceBus;

    #region customize_nsb_host

    class CustomizingHost : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // To customize, use the configuration parameter. 
            // For example, to customize the container:
            busConfiguration.UseContainer<AutofacBuilder>();
        }
    }

    #endregion
}