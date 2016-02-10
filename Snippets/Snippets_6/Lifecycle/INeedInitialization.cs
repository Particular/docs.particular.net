namespace Snippets6.Lifecycle
{
    using NServiceBus;

    #region lifecycle-ineedinitialization

    class NeedsInitialization : INeedInitialization
    {
        public void Customize(EndpointConfiguration configuration)
        {
            // Perform initialization
            // This is after Type Scanning. Do not call the following here:
            // * configuration.ExcludeAssemblies();
            // * configuration.ExcludeTypes();
            // * configuration.ScanAssembliesInNestedDirectories();
        }
    }

    #endregion
}
