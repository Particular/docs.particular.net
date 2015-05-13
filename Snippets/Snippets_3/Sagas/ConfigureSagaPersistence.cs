using NServiceBus;

public class ConfigureSagaPersistence
{

    public void Simple()
    {
        #region saga-configure
        IStartableBus bus = Configure.With()
                                .DefaultBuilder()
                                .MsmqTransport()
                                .Sagas()
                                .UnicastBus()
                                .RavenSagaPersister()
                                .CreateBus();

        #endregion
    }


}
