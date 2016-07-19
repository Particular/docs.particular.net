namespace Core5.Lifecycle
{
    using NServiceBus;
    using NServiceBus.Config;

    #region lifecycle-iwanttorunwhenconfigurationiscomplete

    class RunWhenConfigurationIsComplete :
        IWantToRunWhenConfigurationIsComplete
    {
        public void Run(Configure configure)
        {
            // initialization logic
        }
    }

    #endregion
}
