using NServiceBus;

class ConfiguringWorker
{
    public void Foo()
    {
        #region ConfiguringWorker

        Configure.With()
            .EnlistWithMSMQDistributor();

        #endregion
    }
}