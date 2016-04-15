namespace Snippets4.Lifecycle
{
    using NServiceBus;

    #region lifecycle-ineedinitialization

    class NeedsInitialization : INeedInitialization
    {
        public void Init()
        {
            // Perform initialization logic
        }
    }

    #endregion
}
