namespace Core3.Lifecycle
{
    using NServiceBus;

    #region lifecycle-iwanttorunbeforeconfigurationisfinalized

    class RunBeforeConfigurationIsFinalized : IWantToRunBeforeConfigurationIsFinalized
    {
        public void Run()
        {
            // update config
        }
    }

    #endregion
}
