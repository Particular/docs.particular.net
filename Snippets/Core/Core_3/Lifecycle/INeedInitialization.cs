namespace Core3.Lifecycle
{
    using NServiceBus.Config;

    #region lifecycle-ineedinitialization

    class NeedsInitialization :
        INeedInitialization
    {
        public void Init()
        {
            // Perform initialization logic
        }
    }

    #endregion
}
