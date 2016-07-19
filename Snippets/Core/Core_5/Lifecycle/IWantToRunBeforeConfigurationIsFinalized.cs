namespace Core5.Lifecycle
{
    using NServiceBus;

    #region lifecycle-iwanttorunbeforeconfigurationisfinalized

    class RunBeforeConfigurationIsFinalized :
        IWantToRunBeforeConfigurationIsFinalized
    {
        public void Run(Configure config)
        {
            // update config instance
            config.Settings.Set("key", "value");
            // after this config.Settings will be frozen
        }
    }

    #endregion
}
