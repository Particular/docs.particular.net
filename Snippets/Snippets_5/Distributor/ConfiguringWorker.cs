using NServiceBus;

class ConfiguringWorker
{
    public void Foo()
    {
        #region ConfiguringWorker

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EnlistWithMSMQDistributor();

        #endregion
    }
}