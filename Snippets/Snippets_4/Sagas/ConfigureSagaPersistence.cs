namespace Snippets4.Sagas
{
    using NServiceBus;
    using NServiceBus.Features;

    public class ConfigureSagaPersistence
    {

        public void Simple()
        {
            #region saga-configure

            Feature.Enable<Sagas>();
            Configure configure = Configure.With();
            configure.DefaultBuilder();
            configure.UseTransport<Msmq>();
            configure.UnicastBus();
            configure.RavenSagaPersister();
            IStartableBus bus = configure.CreateBus();

            #endregion
        }

    }
}