using NServiceBus;

class ConfiguringWorker
{
    public void Foo()
    {
        #region ConfiguringWorker
        Configure.With()
            .EnlistWithDistributor();
        #endregion
    }
}
