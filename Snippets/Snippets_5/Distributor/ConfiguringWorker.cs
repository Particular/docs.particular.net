using NServiceBus;

class ConfiguringWorker
{
    public void Foo()
    {
        #region ConfiguringWorker

        BusConfiguration configuration = new BusConfiguration();
        configuration.EnlistWithMSMQDistributor();

        #endregion
    }
}