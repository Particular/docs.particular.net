namespace Core5.Lifecycle
{
    using NServiceBus;

    #region lifecycle-ineedinitialization

    class NeedsInitialization :
        INeedInitialization
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // Perform initialization
            // This is after type scanning.
            // Do NOT call the following here:
            // busConfiguration.AssembliesToScan();
            // busConfiguration.ScanAssembliesInDirectory();
            // busConfiguration.TypesToScan();
        }
    }

    #endregion
}
