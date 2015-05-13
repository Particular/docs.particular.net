using NServiceBus;
using NServiceBus.Features;

public class ConfigureSagaPersistence
{

    public void Simple()
    {
        #region saga-configure
        Feature.Enable<Sagas>();
        IStartableBus bus = Configure.With()
                                .DefaultBuilder()
                                .UseTransport<Msmq>()
                                .UnicastBus()
                                .RavenSagaPersister()
                                .CreateBus();

        #endregion
    }


}
