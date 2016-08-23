namespace Core6.Lifecycle
{
    using NServiceBus;

    #region lifecycle-ineedinitialization

    class NeedsInitialization :
        INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // Perform initialization
            // This is after Type Scanning.
            // Do NOT call the following here:
            // endpointConfiguration.ExcludeAssemblies();
            // endpointConfiguration.ExcludeTypes();
            // endpointConfiguration.ScanAssembliesInNestedDirectories();
        }
    }

    #endregion
}
