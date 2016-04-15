namespace Core4.Lifecycle
{
    using NServiceBus.Config;

    #region lifecycle-iwanttorunwhenconfigurationiscomplete

    class RunWhenConfigurationIsComplete : IWantToRunWhenConfigurationIsComplete
    {
        public void Run()
        {
            // initialization logic            
        }
    }

    #endregion
}
