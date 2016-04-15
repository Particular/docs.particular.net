namespace Core4.Lifecycle
{
    using NServiceBus;

    #region lifecycle-iwanttorunbeforeconfigurationisfinalized

    class RunBeforeConfigurationIsFinalized : IWantToRunBeforeConfigurationIsFinalized
    {
        public void Run()
        {
            // update configuration here
        }
    }

    #endregion
}
